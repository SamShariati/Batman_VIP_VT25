using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEditor.Progress;

public class FindObjectsHUD : MonoBehaviour
{
    
    [SerializeField] DebugIcon template;

    public bool showKeys;
    public bool showRocks;
    public bool showChests;

    ObjectPool<DebugIcon> pool;
    List<KeyTemplatePair> activeKeyIcons = new();
    Camera mainCamera;
    Vector3 camPos ;
    Vector3 camDir;

    List<DebugIcon> activeRockIcons = new();
    List<DebugIcon> activeChestIcons = new();

    struct KeyTemplatePair
    {
        public Key key;
        public DebugIcon rect;
        public string color;
        public KeyTemplatePair(Key key, DebugIcon rect)
        {
            this.key = key;
            this.rect = rect;
            color = key.keyColour;
        }

        internal void SetKey(Key key)
        {
            this.key = key;
            color = key.keyColour;
        }
    }

    void Start()
    {
        
        pool = new(() => { return Instantiate(template, transform); }, (x) => x.gameObject.SetActive(true), (x) => x.gameObject.SetActive(false), (x) => { Destroy(x.gameObject); });
    }

    // Update is called once per frame
    void LateUpdate()
    {
        mainCamera = Camera.main;
        camPos = mainCamera.transform.position;
        camDir = mainCamera.transform.forward;
        if (showKeys)
        {
            UpdateKeysFrame();
        }
        if (showRocks)
        {
            UpdateRocksFrame();
        }
        if (showChests)
        {
            Chest[] rocks = FindObjectsByType<Chest>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            ForEachIcon<Chest>(activeChestIcons, rocks, (item, icon) => UpdateIcon(item.transform.position, icon, item.requiredKey + " chest", Color.yellow));
        }


        if (Input.GetKeyDown(KeyCode.K))
        {
            showKeys = !showKeys;
            if (!showKeys)
            {
                for (int i = activeKeyIcons.Count - 1; i >= 0; i--)
                {
                    pool.Release(activeKeyIcons[i].rect);
                    activeKeyIcons.RemoveAt(i);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            showRocks = !showRocks;
            if (!showRocks)
            {
                ClearExcess(activeRockIcons, 0);
            }
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            showChests = !showChests;
            if (!showChests)
            {
                ClearExcess(activeChestIcons, 0);
            }
        }
    }

    public void UpdateKeyIcon(Key item, int i)
    {
        //Debug.Log(activeIcons[i].color + " read and key is " + activeIcons[i].key);
        if (!item)
        {
            pool.Release(activeKeyIcons[i].rect);
            activeKeyIcons.RemoveAt(i);
            return;
        }

        Vector3 dir = item.transform.position - camPos; //to - from

        float d = Vector3.Dot(camDir, dir); //positiv is infront
        bool behind = d < 0;
        Vector3 screenPos = mainCamera.WorldToScreenPoint(item.transform.position);

        
        activeKeyIcons[i].rect.SetPosition( screenPos);
        activeKeyIcons[i].rect.gameObject.SetActive(!behind);
        activeKeyIcons[i].rect.GetComponentInChildren<TMP_Text>().text = dir.magnitude.ToString("n1") + "m " + item.keyColour;
        
    }

    public void UpdateKeysFrame()
    {
        Key[] keys = FindObjectsByType<Key>(FindObjectsInactive.Exclude, FindObjectsSortMode.None); //this is aahhh!! but debug purposes!!
        for (int i = 0; i < keys.Length; i++)
        {

            Key item = keys[i];
            if (activeKeyIcons.Count == i)
            {
                activeKeyIcons.Add(new(item, pool.Get()));
            }
            activeKeyIcons[i].SetKey(item);
            //Debug.Log(item.keyColour + " key added -> " + activeIcons[i].color);
            UpdateKeyIcon(item, i);
        }
            
        //remove the exess
        for (int i = activeKeyIcons.Count - 1; i >= keys.Length; i--)
        {
            pool.Release(activeKeyIcons[i].rect);
            activeKeyIcons.RemoveAt(i);
        }
        Debug.Assert(activeKeyIcons.Count == keys.Length, "Error");
    }


    public bool UpdateIcon(Vector3 pos, DebugIcon icon, string text, Color color)
    {
        Vector3 dir = pos - camPos; //to - from
        float d = Vector3.Dot(camDir, dir); //positiv is infront
        bool behind = d < 0;
        Vector3 screenPos = mainCamera.WorldToScreenPoint(pos);

        icon.Hide(behind);
        if (!behind)
        {
            icon.SetColor(color);
            icon.SetPosition(screenPos);
            icon.SetText(dir.magnitude.ToString("n1") + "m " + text);
        }
        return !behind;
    }

    public void UpdateRocksFrame()
    {
        Rock[] rocks = FindObjectsByType<Rock>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        ForEachIcon<Rock>(activeRockIcons, rocks, (item,icon) => UpdateIcon(item.transform.position, icon, "Rock", Color.cyan));
        //for (int i = 0; i < rocks.Length; i++)
        //{

        //    Rock item = rocks[i];
        //    if (activeRockIcons.Count == i)
        //    {
        //        activeRockIcons.Add(pool.Get());
        //    }
        //    if(UpdateIcon(item.transform.position, activeRockIcons[i], "Rock", Color.cyan))
        //    {

        //    }
        //}

        ////remove the exess
        //ClearExcess(activeRockIcons, rocks.Length);
        //Debug.Assert(activeRockIcons.Count == rocks.Length, "Error " + activeRockIcons.Count + " " + rocks.Length);
    }

    private void ForEachIcon<T>(List<DebugIcon> activeDebugIcons,  Span<T> objectsOfType, Action<T, DebugIcon> action) where T : MonoBehaviour
    {
        for (int i = 0; i < objectsOfType.Length; i++)
        {

            T item = objectsOfType[i];
            if (activeDebugIcons.Count == i)
            {
                activeDebugIcons.Add(pool.Get());
            }
            action.Invoke(item, activeDebugIcons[i]); 
        }
        //remove the exess
        ClearExcess(activeDebugIcons, objectsOfType.Length);
    }

    private void ClearExcess(List<DebugIcon> icons, int n)
    {
        for (int i = icons.Count - 1; i >= n; i--)
        {
            pool.Release(icons[i]);
            icons.RemoveAt(i);
        }
        //Debug.Assert(icons.Count == n, "Error");
    }
}
