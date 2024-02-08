using UnityEngine;
using UnityEngine.UI;

public class CompassReader : MonoBehaviour
{
    public RectTransform compassNeedle; // �̹����� RectTransform
    public Text compassValueText;   // Compass ���� ǥ���� �ؽ�Ʈ �޽�

    void Start()
    {
        // Compass ���� Ȱ��ȭ
        Input.compass.enabled = true;
    }

    void Update()
    {
        // Compass �������� ������ ��������
        float heading = Input.compass.trueHeading;

        // �̹��� ȸ��
        compassNeedle.localEulerAngles = new Vector3(0f, 0f, -heading);

        // �ؽ�Ʈ�� ������ ǥ��
        compassValueText.text = heading.ToString("F0") + "��";
    }

    void OnDestroy()
    {
        // Compass ���� ��Ȱ��ȭ
        Input.compass.enabled = false;
    }
}
