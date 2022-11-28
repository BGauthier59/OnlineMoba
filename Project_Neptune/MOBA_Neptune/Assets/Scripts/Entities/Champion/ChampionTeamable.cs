using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;

namespace Entities.Champion
{
    public partial class Champion
    {
        // WIP : ITeamable is on EntityFOWViewable.cs
        
        /*
        public new Enums.Team GetTeam()
        {
            return team;
        }

        public new List<Enums.Team> GetEnemyTeams()
        {
            return Enum.GetValues(typeof(Enums.Team)).Cast<Enums.Team>().Where(someTeam => someTeam != team)
                .ToList(); //returns all teams that are not 'team'
        }

        public new bool CanChangeTeam()
        {
            return canChangeTeam;
        }

        public new void RequestChangeTeam(Enums.Team team)
        {
            photonView.RPC("ChangeTeamRPC", RpcTarget.MasterClient, (byte) team);
        }

        [PunRPC]
        public new void ChangeTeamRPC(byte team)
        {
            photonView.RPC("SyncChangeTeamRPC", RpcTarget.All, team);
        }

        [PunRPC]
        public new void SyncChangeTeamRPC(byte team)
        {
            this.team = (Enums.Team) team;
        }


        public event GlobalDelegates.BoolDelegate OnChangeTeam;
        public event GlobalDelegates.BoolDelegate OnChangeTeamFeedback;
        */
    }
}