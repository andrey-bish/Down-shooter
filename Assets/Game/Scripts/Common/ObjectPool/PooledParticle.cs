using Sirenix.OdinInspector;
using UnityEngine;
using static Common.Enums;

namespace Common.ObjectPool
{
    public class PooledParticle: PoolItem
    {
        [SerializeField] private ParticleSystem    _particle;
        [SerializeField] private ReleaseAfterDelay _releaseAfterDelay;
        [SerializeField] private bool              _playOnAwake;
        [SerializeField] protected bool              _isResetScale;
        [SerializeField] private bool _isTrail;
        [SerializeField, ShowIf("_isTrail")] private TrailRenderer _trailRenderer;
        [SerializeField, ShowIf("_isTrail")] private ParticleSystem _particleSystem;
		
		

        public override void Restart()
        {
            base.Restart();
            if (_playOnAwake) _particle.Play();
        }

        public override void Retain(int id, string containerName)
        {
            base.Retain(id, containerName);
            if (_playOnAwake) _particle.Play();
        }

        public virtual void SetParticleType(ParticleType particleType)
        {
            _particle.Play();
        }

        public virtual void SetParticleScale(Vector3 value)
        {
			
        }

        public void SetTrailColor(Gradient gradient, Gradient smoke)
        {
            if (_trailRenderer)
            {
                _trailRenderer.colorGradient = gradient;
            }
            else
            {
                var particleColorOverLifetime = _particle.colorOverLifetime;
                particleColorOverLifetime.color = gradient;
                if (_particleSystem)
                {
                    var particleColorOverLifetimeSmoke = _particleSystem.colorOverLifetime;
                    particleColorOverLifetimeSmoke.color = smoke;
                }
            }
        }

        public override void Release(bool disableObject = true)
        {
            _particle.Stop();
            base.Release(disableObject);
        }

        public void StopRelease() => _releaseAfterDelay.enabled = false;

        public void ReleaseAfter(float delay = 0)
        {
            _releaseAfterDelay.SetDelay(delay);
            _releaseAfterDelay.enabled = true;
        }

        public void Play() => _particle.Play();
        public void Stop() => _particle.Stop();
        public void Pause() => _particle.Pause();
    }
}