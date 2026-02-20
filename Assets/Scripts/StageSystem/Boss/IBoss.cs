namespace StageSystem.Boss
{
public interface IBoss
{
        /// <summary>
        /// ボスのHP
        /// </summary>
        public int HP { get; set; }

        /// <summary>
        /// これが実行されたらボスが起動する
        /// </summary>
        public void BossStart();
        
        /// <summary>
        /// ボスのHPを減らす関数
        /// </summary>
        /// <param name="damage">減らすHP</param>
        public void TakeDamage(int damage);
        
        
}
}
