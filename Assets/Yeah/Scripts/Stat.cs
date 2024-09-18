interface Stat
{
    int maxHealth { get; set; }
    int health { get; set; }
    int damage { get; set; }
    int reward { get; set; }
    void TakeDamage(int damage);
    void Attack(int damage);
}