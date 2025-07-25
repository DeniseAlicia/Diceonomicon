using UnityEngine;
using System.Linq;

public class EnemyAI : MonoBehaviour
{
    public List<DiceSlot> emptySlots = new List<DiceSlot>();
    public Opponent oppponent;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DicePlacement()
    {
        emptySlots.Clear();

        foreach (Enemy enemy in opponent.army)
        {
            foreach (DiceSlot column in enemy.tabletFragment.Columns)
            {
                emptySlots.AddRange(column);
            }
        }

        foreach (Die die in opponent.drawnDice)
        {
            var color = (
                from slot in emptySlots
                where slot.color == die.color
                select slot
            ).ToList();

            int rdm = Random.Range(0, color.Count + 1);
            die.transform = color[rdm].transform;

            var filledSlot = from slot in emptySlots
                             where slot.transform == color[rdm].transform
                             select slot;

            emptySlots.Remove(filledSlot);

            opponent.drawnDice.Remove(die);
            opponent.discardPile.Add(die);
        }
    }
}
