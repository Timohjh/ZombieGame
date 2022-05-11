using UnityEditor;

//Dummy editor to get SO drawer to work
[CanEditMultipleObjects]
[CustomEditor(typeof(UnityEngine.Object), true)]
public class UnityObjectEditor : Editor
{
}
