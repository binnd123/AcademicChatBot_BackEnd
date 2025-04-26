using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;

namespace AcademicChatBot.Service.Implementation
{
    public class POMappingPLOService : IPOMappingPLOService
    {
        private readonly IGenericRepository<ProgramingLearningOutcome> _pLORepository;
        private readonly IGenericRepository<ProgramingOutcome> _pORepository;
        private readonly IGenericRepository<POMappingPLO> _pOMappingPLORepository;
        private readonly IUnitOfWork _unitOfWork;

        public POMappingPLOService(IGenericRepository<ProgramingLearningOutcome> pLORepository, IGenericRepository<ProgramingOutcome> pORepository, IGenericRepository<POMappingPLO> pOMappingPLORepository, IUnitOfWork unitOfWork)
        {
            _pLORepository = pLORepository;
            _pORepository = pORepository;
            _pOMappingPLORepository = pOMappingPLORepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> AddPLOsToPO(Guid pOId, List<Guid> pLOIds)
        {
            Response dto = new Response();
            try
            {
                var pO = await _pORepository.GetById(pOId);
                if (pO == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "PO not found";
                    return dto;
                }
                var pOMappingPLOList = new List<POMappingPLO>();
                foreach (var pLOId in pLOIds)
                {
                    var pLO = await _pLORepository.GetById(pLOId);
                    if (pLO == null) continue;
                    var existingPOMappingPLO = await _pOMappingPLORepository.GetFirstByExpression(x => x.ProgramingLearningOutcomeId == pLOId && x.ProgramingOutcomeId == pOId);
                    if (existingPOMappingPLO != null) continue;
                    pOMappingPLOList.Add(new POMappingPLO
                    {
                        POMappingPLOId = Guid.NewGuid(),
                        ProgramingLearningOutcomeId = pLOId,
                        ProgramingOutcomeId = pOId,
                    });
                }
                await _pOMappingPLORepository.InsertRange(pOMappingPLOList);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = pOMappingPLOList;
                dto.Message = "Add PO Mapping PLO successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while Add PO Mapping PLO: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> AddPOsToPLO(Guid pLOId, List<Guid> pOIds)
        {
            Response dto = new Response();
            try
            {
                var pLO = await _pLORepository.GetById(pLOId);
                if (pLO == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "PLO not found";
                    return dto;
                }
                var pOMappingPLOList = new List<POMappingPLO>();
                foreach (var pOId in pOIds)
                {
                    var pO = await _pORepository.GetById(pOId);
                    if (pO == null) continue;
                    var existingPOMappingPLO = await _pOMappingPLORepository.GetFirstByExpression(x => x.ProgramingLearningOutcomeId == pLOId && x.ProgramingOutcomeId == pOId);
                    if (existingPOMappingPLO != null) continue;
                    pOMappingPLOList.Add(new POMappingPLO
                    {
                        POMappingPLOId = Guid.NewGuid(),
                        ProgramingLearningOutcomeId = pLOId,
                        ProgramingOutcomeId = pOId,
                    });
                }
                await _pOMappingPLORepository.InsertRange(pOMappingPLOList);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = pOMappingPLOList;
                dto.Message = "Add PO Mapping PLO successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while Add PO Mapping PLO: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeleteAllPLOsFromPO(Guid pOId)
        {
            var dto = new Response();
            try
            {
                var pO = await _pORepository.GetById(pOId);
                if (pO == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "PO not found";
                    return dto;
                }
                var pOMappingPLOs = await _pOMappingPLORepository.GetAllDataByExpression(
                    filter: x => x.ProgramingOutcomeId == pOId,
                    pageNumber: 1,
                    pageSize: int.MaxValue,
                    orderBy: null,
                    isAscending: true,
                    includes: null);
                if (pOMappingPLOs == null || !pOMappingPLOs.Items.Any())
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "No PLO found for the specified PO.";
                    return dto;
                }
                await _pOMappingPLORepository.DeleteRange(pOMappingPLOs.Items);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Message = "All PLOs have been deleted from the PO.";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting PLOs from the PO: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeleteAllPOsFromPLO(Guid pLOId)
        {
            var dto = new Response();
            try
            {
                var pLO = await _pLORepository.GetById(pLOId);
                if (pLO == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "PLO not found";
                    return dto;
                }
                var pOMappingPLOs = await _pOMappingPLORepository.GetAllDataByExpression(
                    filter: x => x.ProgramingLearningOutcomeId == pLOId,
                    pageNumber: 1,
                    pageSize: int.MaxValue,
                    orderBy: null,
                    isAscending: true,
                    includes: null);
                if (pOMappingPLOs == null || !pOMappingPLOs.Items.Any())
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "No PO found for the specified PLO.";
                    return dto;
                }
                await _pOMappingPLORepository.DeleteRange(pOMappingPLOs.Items);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Message = "All POs have been deleted from the PLO.";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting PLOs from the PO: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeletePLOsFromPO(Guid pOId, List<Guid> pLOIds)
        {
            var dto = new Response();
            try
            {
                var pO = await _pORepository.GetById(pOId);
                if (pO == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "PO not found.";
                    return dto;
                }

                var pOMappingPLOs = await _pOMappingPLORepository.GetAllDataByExpression(
                    filter: x => x.ProgramingOutcomeId == pOId && pLOIds.Contains((Guid)x.ProgramingLearningOutcomeId),
                    pageNumber: 1,
                    pageSize: int.MaxValue,
                    orderBy: null,
                    isAscending: true,
                    includes: null);

                if (pOMappingPLOs == null || !pOMappingPLOs.Items.Any())
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "No matching PLOs links found for the PO.";
                    return dto;
                }

                await _pOMappingPLORepository.DeleteRange(pOMappingPLOs.Items);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Data = pOMappingPLOs.Items.Count;
                dto.Message = "PLOs have been deleted from the PO.";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting PLOs from the PO: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeletePOsFromPLO(Guid pLOId, List<Guid> pOIds)
        {
            var dto = new Response();
            try
            {
                var pLO = await _pLORepository.GetById(pLOId);
                if (pLO == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "PLO not found.";
                    return dto;
                }

                var pOMappingPLOs = await _pOMappingPLORepository.GetAllDataByExpression(
                    filter: x => x.ProgramingLearningOutcomeId == pLOId && pOIds.Contains((Guid)x.ProgramingOutcomeId),
                    pageNumber: 1,
                    pageSize: int.MaxValue,
                    orderBy: null,
                    isAscending: true,
                    includes: null);

                if (pOMappingPLOs == null || !pOMappingPLOs.Items.Any())
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "No matching POs links found for the PLO.";
                    return dto;
                }

                await _pOMappingPLORepository.DeleteRange(pOMappingPLOs.Items);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Data = pOMappingPLOs.Items.Count;
                dto.Message = "POs have been deleted from the PLO.";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting POs from the PLO: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetAllPLOsForPO(Guid POId, int pageNumber, int pageSize)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _pOMappingPLORepository.GetAllDataByExpression(
                    filter: t => t.ProgramingOutcomeId == POId,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: null,
                    isAscending: true,
                    includes: new Expression<Func<POMappingPLO, object>>[]
                {
                    c => c.ProgramingOutcome,
                    c => c.ProgramingLearningOutcome
                });
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "PLOs For PO retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving PLOs For PO: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetAllPOsForPLO(Guid PLOId, int pageNumber, int pageSize)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _pOMappingPLORepository.GetAllDataByExpression(
                    filter: t => t.ProgramingLearningOutcomeId == PLOId,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: null,
                    isAscending: true,
                    includes: new Expression<Func<POMappingPLO, object>>[]
                {
                    c => c.ProgramingOutcome,
                    c => c.ProgramingLearningOutcome
                });
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "POs For PLO retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving POs For PLO: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetPOMappingPLOById(Guid pOMappingPLOId)
        {
            Response dto = new Response();
            try
            {
                var pOMappingPLO = await _pOMappingPLORepository.GetById(pOMappingPLOId);
                if (pOMappingPLO == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "PO Mapping PLO not found";
                    return dto;
                }
                dto.Data = pOMappingPLO;
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "PO Mapping PLO retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving PO Mapping PLO: " + ex.Message;
            }
            return dto;
        }
    }
}
