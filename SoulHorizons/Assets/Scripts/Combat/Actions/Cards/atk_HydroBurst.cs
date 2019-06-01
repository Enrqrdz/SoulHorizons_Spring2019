using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Hydro Burst")]
public class atk_HydroBurst : AttackData
{
    public ActionData HydroBurst;
    public float dampenLength;
    private Vector2Int gridPosition;
    public List<Entity> entityHit = new List<Entity>();
    private int hitCounter = 0;
    private bool hitCounterStarted = false;

    public override Vector2Int ProgressAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        return new Vector2Int(xPos + 1, yPos);
    }

    public override void LaunchEffects(ActiveAttack activeAttack)
    {
        activeAttack.particle = Instantiate(particles, scr_Grid.GridController.GetWorldLocation(activeAttack.position.x, activeAttack.position.y) + particlesOffset, Quaternion.identity);
        activeAttack.particle.sortingOrder = -activeAttack.position.y;

    }

    public override void ProgressEffects(ActiveAttack activeAttack)
    {
        gridPosition = activeAttack.position;
        activeAttack.particle.transform.position = Vector3.Lerp(activeAttack.particle.transform.position, scr_Grid.GridController.GetWorldLocation(activeAttack.lastPosition.x, activeAttack.lastPosition.y) + activeAttack.attack.particlesOffset, (particleSpeed) * Time.deltaTime);
    }

    public override void ImpactEffects(int xPos = -1, int yPos = -1)
    {
        entityHit.Add(scr_Grid.GridController.GetEntityAtPosition(gridPosition.x, gridPosition.y));

        try
        {
            entityHit[entityHit.Count - 1].StartCoroutine(Dampen(dampenLength));
            entityHit[entityHit.Count - 1].StartCoroutine(HitCounter());

            damage *= 2;
            hitCounter++;
        }
        catch
        {
            damage = 4;
        }
    }

    public IEnumerator HitCounter()
    {
        hitCounterStarted = true;
        yield return new WaitForSeconds(1.75f);
        hitCounterStarted = false;
        damage = 4;
        hitCounter = 0;
    }

    public IEnumerator Dampen(float time)
    {
        entityHit[entityHit.Count - 1].isDampened = true;
        yield return new WaitForSeconds(time);
        try
        {
            foreach (Entity entity in entityHit)
            {
                entity.isDampened = false;
            }
        }
        catch
        {

        }
        entityHit.Clear();
    }

    public override void EndEffects(ActiveAttack activeAttack)
    {

    }
}
