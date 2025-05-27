using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour
{
    public MicrophoneData microphoneData;

    public TMP_Dropdown dropdown;
    public TMP_InputField maxLoudnessInputField;
    public TMP_InputField micThresholdInputField;
    public TMP_InputField micBoostInputField;
    void Start()
    {
        maxLoudnessInputField.characterValidation = TMP_InputField.CharacterValidation.Decimal;
        maxLoudnessInputField.text = microphoneData.maxLoudness.ToString();
        maxLoudnessInputField.onSubmit.AddListener(delegate {
            float ml = microphoneData.maxLoudness;
            if(float.TryParse(maxLoudnessInputField.text, out float f)){
                microphoneData.maxLoudness = f;
                microphoneData.Save();
            }
            else
            {
                maxLoudnessInputField.text = ml.ToString();
            }
        });

        micThresholdInputField.characterValidation = TMP_InputField.CharacterValidation.Decimal;
        micThresholdInputField.text = microphoneData.threshold.ToString();
        micThresholdInputField.onSubmit.AddListener(delegate {
            float ml = microphoneData.threshold;
            if (float.TryParse(micThresholdInputField.text, out float f))
            {
                microphoneData.threshold = f;
                microphoneData.Save();
            }
            else
            {
                micThresholdInputField.text = ml.ToString();
            }
        });

        micBoostInputField.characterValidation = TMP_InputField.CharacterValidation.Decimal;
        micBoostInputField.text = microphoneData.micBoost.ToString();
        micBoostInputField.onSubmit.AddListener(delegate {
            float ml = microphoneData.micBoost;
            if (float.TryParse(micBoostInputField.text, out float f))
            {
                microphoneData.micBoost = f;
                microphoneData.Save();
            }
            else
            {
                micBoostInputField.text = ml.ToString();
            }
        });


        dropdown.ClearOptions();
        List<string> lst = new(microphoneData.avalibleMicrophones);
        dropdown.AddOptions(lst);
        for (int i = 0; i < lst.Count; i++)
        {
            if (microphoneData.microphone == lst[i])
            {
                dropdown.value = i;
            }
        }
        
        
        //() => microphoneData.microphone = dropdown.options[dropdown.value].text
        dropdown.onValueChanged.AddListener(delegate {
            microphoneData.microphone = dropdown.options[dropdown.value].text;
            microphoneData.Save();
        });
    }


    public void ResetSettings()
    {
        MicrophoneData defaultValues = ScriptableObject.CreateInstance<MicrophoneData>();

        //Debug.Log("Boost " + defaultValues.micBoost);
        //Debug.Log("maxLoudness " + defaultValues.maxLoudness);
        //Debug.Log("threshold " + defaultValues.threshold);
        //Debug.Log("Boost " + defaultValues.micBoost);

        microphoneData.threshold = defaultValues.threshold;
        microphoneData.maxLoudness = defaultValues.maxLoudness;
        microphoneData.micBoost = defaultValues.micBoost;
        micBoostInputField.text = microphoneData.micBoost.ToString();
        micThresholdInputField.text = microphoneData.threshold.ToString();
        maxLoudnessInputField.text = microphoneData.maxLoudness.ToString();

        Destroy(defaultValues);
        microphoneData.Save();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
