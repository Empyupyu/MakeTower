using UnityEngine;

namespace Source.Scripts.Interface
{
    public interface IDropZone
    {
        public void Drop(IDrop drop);
    }

    public interface IDrop
    {
        //TODO
        public GameObject GetGameObject();
    }
}