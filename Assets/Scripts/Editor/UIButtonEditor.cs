using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(UIButton), true)]
[CanEditMultipleObjects]
public class UIButtonEditor : ButtonEditor
{
    // 프로퍼티를 담을 변수 선언
    SerializedProperty clickSoundProp;
    SerializedProperty hoverSoundProp;
    SerializedProperty onClickedProp;

    protected override void OnEnable()
    {
        base.OnEnable();
        // 실제 스크립트의 변수명과 일치해야 합니다 (문자열 주의)
        clickSoundProp = serializedObject.FindProperty("clickSound");
        hoverSoundProp = serializedObject.FindProperty("hoverSound");
        onClickedProp = serializedObject.FindProperty("OnClicked");
    }

    public override void OnInspectorGUI()
    {
        // 1. 데이터 업데이트 시작
        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Sound Settings", EditorStyles.boldLabel);

        // 2. 사운드 필드 그리기
        EditorGUILayout.PropertyField(clickSoundProp);
        EditorGUILayout.PropertyField(hoverSoundProp);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Custom Events", EditorStyles.boldLabel);

        // 3. UnityEvent(OnClicked) 그리기
        EditorGUILayout.PropertyField(onClickedProp);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Button Settings", EditorStyles.boldLabel);

        // 4. 기존 Button의 기본 속성들 그리기
        base.OnInspectorGUI();

        // 5. 변경사항 저장
        serializedObject.ApplyModifiedProperties();
    }
}