using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace StageSystem.Boss
{
public class BossAttackBehavior : MonoBehaviour
{
    [SerializeField] GameObject shotArea;

    [SerializeField] GameObject shotArea2;
    
    [SerializeField] GameObject chock;
    [SerializeField] GameObject pen; 
    [SerializeField] GameObject attendanceRecord;

    BossManager _bossManager;
    void Awake()
    {
        _bossManager = GetComponent<BossManager>();
    }
    
    public void ChockShot()
    {
        var chalk = Instantiate(chock, shotArea.transform.position, Quaternion.Euler(0f,90f,0f));
        chalk.GetComponent<Chalk>().Init(gameObject, _bossManager);
    }

    public void PenShot()
    {
        Instantiate(pen, shotArea.transform.position, Quaternion.Euler(0f,90f,0f));
    }
    
    public void AttendanceRecordShot()
    {
        Instantiate(attendanceRecord,shotArea.transform.position, Quaternion.Euler(90f,90f,0f));
    }
    
    public IEnumerator AttendanceRecordAndChockShot()
    {
        AttendanceRecordShot();
        yield return new WaitForSeconds(1f);
        var chalk = Instantiate(chock, shotArea2.transform.position, Quaternion.Euler(0f,90f,0f));
        chalk.GetComponent<Chalk>().Init(gameObject, _bossManager);
    }
}
}