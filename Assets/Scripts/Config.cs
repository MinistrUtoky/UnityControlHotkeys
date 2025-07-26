using UnityEngine;

namespace Source.Scripts.ConfigSystem
{
    public class Config : ConfigsAbstraction
    {
        public static Config Instance { get; private set; }

        [SerializeField] private int afk;
        [SerializeField] private float delay;

        private const string nameAFK = "afkControl";
        private const string nameDelay = "interlevelDelay";
        
        private const string descriptionAFK = "Значения 0 или 1. Ведет ли неактивность в 30 секунд к выходу в экран ожидания (0 - нет, 1 - да).";
        private const string descriptionDelay = "Время в целых секундах для показа информации между уровнями. Не может быть меньше 2 секунд.";

        public int AFK => afk;
        public float InterlevelDelay => delay;

        private void Start()
        {
            Instance = this;
        }

        protected override void InitFields()
        {
            afk = (int)GetValueOrSetDefault(nameAFK, descriptionAFK, afk);
            delay = GetValueOrSetDefault(nameDelay, descriptionDelay, delay);
        }
    }
}