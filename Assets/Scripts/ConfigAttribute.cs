namespace Source.Scripts.ConfigSystem
{
    public class ConfigAttribute
    {
        public ConfigAttribute(float value, string description)
        {
            DefaultValue = value;
            DefaultDescription = description;
        }
        
        public float DefaultValue { get; }

        public string DefaultDescription { get; }
    }
}