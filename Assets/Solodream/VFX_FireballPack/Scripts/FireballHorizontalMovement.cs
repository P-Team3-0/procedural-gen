using UnityEngine;
using UnityEngine.VFX;

namespace FireballMovement
{    public class FireballHorizontalMovement : MonoBehaviour
    {
        [SerializeField] private Vector3 _startingPos;
        [SerializeField] private float _moveSpeed = 0.1f;
        [SerializeField] private float delay = 6f;
        [SerializeField] private float timer = 0f;

        private Rigidbody _fireball;
        private VisualEffect _fireballTrails;

        private const string _fireballTrailsActiveString = "FireballTrailsActive";

        void Start()
        {           
            _fireball = gameObject.GetComponent<Rigidbody>();
            _fireballTrails = gameObject.GetComponent<VisualEffect>();
            _startingPos = _fireball.transform.localPosition;
        }

        void Update()
        {
            if (timer < delay)
            {
                timer += Time.deltaTime;
                _fireballTrails.enabled = true;
                _fireball.AddRelativeForce(new Vector3(_moveSpeed, 0, 0));
            }
            else if (timer >= delay)
            {
                ResetAll();
            }

            if (_fireball.velocity.x > 1 || _fireball.velocity.x < -1)
            {
                _fireballTrails.SetBool(_fireballTrailsActiveString, true);
            }
            else
            {
                _fireballTrails.SetBool(_fireballTrailsActiveString, false);
            }
        }

        private void ResetAll()
        {
            _fireballTrails.enabled = false;
            _fireball.velocity = Vector3.zero;
            _fireball.transform.localPosition = _startingPos;
            timer = 0;
        }
    }
}