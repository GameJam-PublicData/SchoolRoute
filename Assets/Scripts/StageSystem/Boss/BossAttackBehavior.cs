using UnityEngine;
using UnityEngine.Serialization;

namespace StageSystem.Boss
{
public class BossAttackBehavior : MonoBehaviour
{
    [SerializeField] GameObject chock;
    [SerializeField] GameObject pen; 
    [SerializeField] GameObject attendanceRecord;

    public void ChockShot()
    {
        Instantiate(chock, transform.position, Quaternion.identity);
    }

    public void PenShot()
    {
        Instantiate(pen, transform.position, Quaternion.identity);
    }
    
    public void AttendanceRecordShot()
    {
        Instantiate(attendanceRecord, transform.position, Quaternion.identity);
    }
}
}