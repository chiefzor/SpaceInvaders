using System;
using Microsoft.Xna.Framework.Audio;

namespace Infrastructure.ServiceInterfaces
{
    public interface ISoundManager
    {
        float BackgroundMusicVolume { get; set; }

        float SoundEffectsVolume { get; set; }

        bool ToggleSound { get; set; }

        void PlaySound(string i_AssetName, float i_Pitch = 0, float i_Pan = 0);

        SoundEffectInstance PlayContinuousSound(string i_AssetName, bool i_IsBackgroundMusic = false);

        event EventHandler<EventArgs> ToggleSoundChanged;
    }
}