using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Client.Battle
{
    public class SkidEffect
    {
        private readonly TrailRenderer _skidMarkRenderer;
        private readonly ParticleSystem _skidSmokeParticles;
        
        public SkidEffect(TrailRenderer skidMarkRenderer, ParticleSystem skidSmokeParticles)
        {
            _skidMarkRenderer = skidMarkRenderer;
            _skidSmokeParticles = skidSmokeParticles;
        }

        public bool IsEnabled
        {
            set
            {
                if (value)
                {
                    _skidMarkRenderer.emitting = true;
                    if (!_skidSmokeParticles.isPlaying)
                    {
                        _skidSmokeParticles.Play();    
                    }
                }
                else
                {
                    _skidMarkRenderer.emitting = false;
                    if (_skidSmokeParticles.isPlaying)
                    {
                        _skidSmokeParticles.Stop();    
                    }
                }
            }
        }
        
    }
}