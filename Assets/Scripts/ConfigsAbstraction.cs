using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Source.Scripts.ConfigSystem
{
    public abstract class ConfigsAbstraction : MonoBehaviour
    {
        private string _configPath = "config.txt";
        private string[] _configLines;
        private string _hash;
        private string _resultPath => Application.streamingAssetsPath + "/" + _configPath; 
        private bool _isInitialized;
        private Dictionary<string, ConfigAttribute> _defaultValues = new Dictionary<string, ConfigAttribute>();
        
        public void Initialize()
        {
            _isInitialized = false;
            LoadConfig();
            InitFields();
            _isInitialized = true;

        }
        
        protected abstract void InitFields();

        private void CheckHashSum()
        {
            var hash = GetConfigHash();

            if (hash != _hash)
            {
                LoadConfig();
                _hash = hash;
                InitFields();
                
            }
        }
        
        private string GetConfigHash()
        {
            var filePath = _resultPath;
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(filePath);
            return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
        }
        
        private void LoadConfig()
        {
            _configLines = File.ReadAllLines(_resultPath);
        }
        
        private void SaveNewLineToConfig(string value)
        {
            if (File.Exists(_resultPath))
            {
                _configLines = File.ReadAllLines(_resultPath);
                using var sw = File.AppendText(_resultPath);
                sw.WriteLine(value);
            }
            else
            {
                Debug.LogError($"Config file not found: {_resultPath}.");
            }

            _hash = GetConfigHash();
        }
        
        protected float GetValueOrSetDefault(string valueName, string description, float defaultValue)
        {
            if (!_isInitialized)
            {
                var configAttribute = new ConfigAttribute(defaultValue, description);
                _defaultValues[valueName] = configAttribute;
            }
            
            Debug.Log($"{valueName} : {defaultValue}");
            float value = 0;
            bool isFound = false;
            foreach (string line in _configLines)
            {
                var rawLine = line.Trim().Split('#');
                var rawData = rawLine[0].Trim().Split('=');
                if (!Validate(rawData)) continue;
                
                var data = new[]{ rawData[0].Trim(), rawData[1].Trim() };
            
                if (string.Equals(data[0], valueName, StringComparison.CurrentCultureIgnoreCase))
                {
                    var resultString = data[1].Replace(".", ",");
                    value = float.Parse(resultString);
                    isFound = true;
                    break;
                }
            }
            if (!isFound)
            {
                var resultLine = $"{valueName}={_defaultValues[valueName].DefaultValue}{GetEmptySpaces(valueName)}# {_defaultValues[valueName].DefaultDescription}";
                SaveNewLineToConfig(resultLine);
                value = _defaultValues[valueName].DefaultValue;
            }
        
            return value;
        }

        private string GetEmptySpaces(string valueName)
        {
            var holdingLetters = 31 - valueName.Length - _defaultValues[valueName].DefaultValue.ToString(CultureInfo.InvariantCulture).Length;
            var result = new StringBuilder().Insert(0, " ", holdingLetters).ToString(); 
            return result;
        }
        
        private bool Validate(string[] rawData)
        {
            if (rawData.Length < 2) return false;
            if (string.IsNullOrEmpty(rawData[0])) return false;
            if (rawData[0][0] == '[') return false;
            return true;
        }
        
        public float GetValue(string valueName)
        {
            float value = 0;
            bool isFound = false;

            foreach (string line in _configLines)
            {
                var rawLine = line.Trim().Split('#');
                var rawData = rawLine[0].Trim().Split('=');
                if (!Validate(rawData)) continue;
            
                var data = new[]{ rawData[0].Trim(), rawData[1].Trim() };
            
                if (string.Equals(data[0], valueName, StringComparison.CurrentCultureIgnoreCase))
                {
                    var resultString = data[1].Replace(".", ",");
                    value = float.Parse(resultString);
                    isFound = true;
                    break;
                }
            }

            if (!isFound)
            {
                value = GetValueOrSetDefault(valueName, _defaultValues[valueName].DefaultDescription, _defaultValues[valueName].DefaultValue);
            }
            
            return value;
        }

        private void FixedUpdate()
        {
            CheckHashSum();
        }
    }
}