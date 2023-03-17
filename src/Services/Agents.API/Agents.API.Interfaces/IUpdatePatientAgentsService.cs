using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Interfaces
{
    public interface IUpdatePatientAgentsService
    {
        /// <summary>
        /// Обновить состояния агентов пациентов.
        /// </summary>
        /// <param name="updateInfo"></param>
        /// <returns>Количество успешно обновленных агентов.</returns>
        public Task<int> UpdatePatientAgents(IUpdatePatientsDataInfo updateInfo);
    }
}
