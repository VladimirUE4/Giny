using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Triggers
{
    public enum TriggerType
    {
        Instant, // I

        OnDamaged, // D
        OnDamagedAir, // DA
        OnDamagedEarth, // DE
        OnDamagedFire, // DF
        OnDamagedWater, // DW
        OnDamagedNeutral, // DN
        OnDamagedByAlly, // DBA
        OnDamagedByEnemy, // DBE
        OnDamagedBySummon, // DI
        OnDamagedByWeapon, // DC
        OnDamagedBySpell, // DS
        OnDamagedByGlyph, // DG
        OnDamagedByTrap, //DP
        OnDamagedInCloseRange, //DM
        OnDamagedInLongRange, //DR
        OnDamagedByPush, // MD-->PD
        OnDamagedByAllyPush, //PMD
        OnDamageEnemyByPush, // MMD
        OnDamagedUnknown_2, // Dr
        OnDamagedUnknown_3, // DTB
        OnDamagedUnknown_4, // DTE

        OnSummon, // CI

        OnTurnBegin, // TB
        OnTurnEnd, // TE

        OnMPLost, // m , MPA (effective loss? attempt? )
        OnAPLost, // A
        OnRangeLost, // R

        OnHealed, //H

        OnStateAdded, // EO
        OnSpecificStateAdded, //EO#
        OnStateRemoved, //Eo
        OnSpecificStateRemoved, //Eo#

        OnDispelled, //d

        OnCriticalHit, //CC
        OnDeath, //X

        OnPushed, //MP , P
        OnMoved, //M
        OnTackled, //tF
        OnTackle, //tS

        /* CUSTOM */

        AfterDamaged,
        Delayed,
        Unknown,
        /*
        *A=lose AP (101)
        *CC=on critical hit
        *d=dispell
        *D=damage
        *DA=damage air
        *DBA=damage on ally
        *DBE=damage on enemy
        *DC=damaged by weapon
        *DE=damage earth
        *DF=damage fire
        *DG=damage from glyph
        *DI=
        *DM=distance between 0 and 1
        *DN=damage neutral
        *DP=damage from trap
        *Dr=
        *DR=distance > 1
        *DS=not weapon
        *DTB=
        *DTE=
        *DW=damage water
        *EO=on add state
        *EO#=on add state #
        *Eo=on state removed
        *Eo#=on state # removed
        *H=on heal
        *I=instant
        *m=lose mp (127)
        *M=OnMoved
        *mA=
        *MD=push damage
        *MDM=receive push damages from enemy push
        *MDP=inflict push damage to enemy
        *ML=
        *MP=Pushed
        *MS=
        *P=
        *R=Lost Range
        *TB=turn begin
        *TE=turn end
        *tF=Tackled
        *tS=Tackle
        *X= Death
        *CT =tackle enemy?
        *CI = Summoned
        */
    }
}
