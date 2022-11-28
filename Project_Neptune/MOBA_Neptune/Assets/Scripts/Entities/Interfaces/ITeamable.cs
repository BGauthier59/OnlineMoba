using System.Collections.Generic;

namespace Entities
{
    public interface ITeamable
    {
        /// <returns>the team of the entity</returns>
        public Enums.Team GetTeam();
        /// <returns>returns the teams that the entity considers its enemy</returns>
        public List<Enums.Team> GetEnemyTeams();
        
        /// <returns>true if the entity can change team, false if not</returns>
        public bool CanChangeTeam();
        /// <summary>
        /// Sends an RPC to the master to change the entity's team.
        /// </summary>
        public void RequestChangeTeam(Enums.Team team);
        /// <summary>
        /// Sends an RPC to all clients to set the entity's team.
        /// </summary>
        public void SyncChangeTeamRPC(byte team);
        /// <summary>
        /// Sets the entity's team.
        /// </summary>
        public void ChangeTeamRPC(byte team);

        public event GlobalDelegates.BoolDelegate OnChangeTeam;
        public event GlobalDelegates.BoolDelegate OnChangeTeamFeedback;
    }
}