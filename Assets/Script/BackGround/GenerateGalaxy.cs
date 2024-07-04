using UnityEngine;

public class GenerateGalaxy : MonoBehaviour
{
    [SerializeField] private Transform player, BG1, BG2, BG3, BG4;
    [SerializeField] private Vector2 size;

    private Vector3 BG1TargetPos = new Vector3();
    private Vector3 BG2TargetPos = new Vector3();
    private Vector3 BG3TargetPos = new Vector3();
    private Vector3 BG4TargetPos = new Vector3();

    void FixedUpdate()
    {
        if (transform.position.y >= BG2.position.y && transform.position.x >= BG2.position.x)
        {
            BG1.position = SetPos(BG1TargetPos, BG1.position.x, BG2.position.y + size.y, BG1.position.z);
            BG3.position = SetPos(BG3TargetPos, BG2.position.x + size.x, BG2.position.y + size.y, BG1.position.z);
            BG4.position = SetPos(BG4TargetPos, BG2.position.x + size.x, BG4.position.y, BG1.position.z);
            SwitchingBG();
            SwitchBg2();

        }
        if (transform.position.y < BG2.position.y && transform.position.x >= BG2.position.x)
        {
            BG1.position = SetPos(BG2TargetPos, BG1.position.x, BG2.position.y - size.y, BG1.position.z);
            BG3.position = SetPos(BG3TargetPos, BG2.position.x + size.x, BG2.position.y - size.y, BG1.position.z);
            BG4.position = SetPos(BG4TargetPos, BG2.position.x + size.x, BG4.position.y, BG1.position.z);
            SwitchBg5();
            SwitchBg6();

        }
        if (transform.position.y > BG2.position.y && transform.position.x < BG2.position.x)
        {
            BG1.position = SetPos(BG2TargetPos, BG1.position.x, BG2.position.y + size.y, BG1.position.z);
            BG3.position = SetPos(BG3TargetPos, BG2.position.x - size.x, BG2.position.y + size.y, BG1.position.z);
            BG4.position = SetPos(BG4TargetPos, BG2.position.x - size.x, BG4.position.y, BG1.position.z);
            SwitchBg3();
            SwitchBg4();
        }
        if (transform.position.y < BG2.position.y && transform.position.x < BG2.position.x)
        {
            BG1.position = SetPos(BG2TargetPos, BG1.position.x, BG2.position.y - size.y, BG1.position.z);
            BG3.position = SetPos(BG3TargetPos, BG2.position.x - size.x, BG2.position.y - size.y, BG1.position.z);
            BG4.position = SetPos(BG4TargetPos, BG2.position.x - size.x, BG4.position.y, BG1.position.z);
            SwitchBg3();
            SwitchBg4();
        }

    }

    private void SwitchingBG()
    {
        Transform temp = BG1;
        BG1 = BG2;
        BG2 = temp;
    }

    private void SwitchBg2()
    {
        Transform temp = BG3;
        BG3 = BG4;
        BG4 = temp;
    }
    private void SwitchBg3()
    {
        Transform temp = BG1;
        BG1 = BG3;
        BG3 = temp;
    }
    private void SwitchBg4()
    {
        Transform temp = BG2;
        BG2 = BG4;
        BG4 = temp;
    }
    private void SwitchBg5()
    {
        Transform temp = BG1;
        BG1 = BG4;
        BG4 = temp;
    }
    private void SwitchBg6()
    {
        Transform temp = BG2;
        BG2 = BG3;
        BG3 = temp;
    }

    private Vector3 SetPos(Vector3 pos, float x, float y, float z)
    {
        pos.x = x;
        pos.y = y;
        pos.z = z;
        return pos;
    }
}
