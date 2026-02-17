namespace StageSystem.Boss
{
public interface IBoss
{
        /// <summary>
        /// ボスのHP
        /// </summary>
        public int HP { get; set; }
        
        /// <summary>
        /// ボスのHPを減らす関数
        /// </summary>
        /// <param name="damage">減らすHP</param>
        public void TakeDamage(int damage);
        
        /// <summary>
        /// ボスの攻撃方法
        /// </summary>
        public void Attack();
        
}
}
