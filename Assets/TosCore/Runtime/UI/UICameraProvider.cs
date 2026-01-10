using UnityEngine;

namespace TosCore.UI
{
    public class UICameraProvider : MonoBehaviour
    {
        private static UICameraProvider _instance;
        
        [SerializeField] private Camera _camera; // UICamera
        
        public static UICameraProvider Instance => _instance;
        
        public Camera Camera => _camera;


        private void Awake()
        {
            _instance = this;
        }
    }
}