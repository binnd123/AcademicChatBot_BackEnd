using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;

namespace AcademicChatBot.Service.Implementation
{
    public class ComboSubjectService : IComboSubjectService
    {
        private readonly IGenericRepository<Subject> _subjectRepository;
        private readonly IGenericRepository<Combo> _comboRepository;
        private readonly IGenericRepository<ComboSubject> _comboSubjectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ComboSubjectService(IGenericRepository<Subject> subjectRepository, IGenericRepository<Combo> comboRepository, IGenericRepository<ComboSubject> comboSubjectRepository, IUnitOfWork unitOfWork)
        {
            _subjectRepository = subjectRepository;
            _comboRepository = comboRepository;
            _comboSubjectRepository = comboSubjectRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> AddComboSubject(Guid comboId, Guid subjectId, int semesterNo, string? note)
        {
            Response dto = new Response();
            try
            {
                if(semesterNo < 4 || semesterNo > 9)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.EXCEPTION;
                    dto.Message = "Semester number must be greater than 3 and lower than 10";
                    return dto;
                }
                var tool = await _comboRepository.GetById(comboId);
                if (tool == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Combo not found";
                    return dto;
                }
                var subject = await _subjectRepository.GetById(subjectId);
                if (tool == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Subject not found";
                    return dto;
                }
                var existingComboSubject = await _comboSubjectRepository.GetFirstByExpression(x => x.ComboId == comboId && x.SubjectId == subjectId);
                if (existingComboSubject != null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.EXCEPTION;
                    dto.Message = "Combo Subject is already exist";
                    return dto;
                }
                var comboSubject = new ComboSubject
                {
                    ComboSubjectId = Guid.NewGuid(),
                    ComboId = comboId,
                    SubjectId = subjectId,
                    SemesterNo = semesterNo,
                    Note = note,
                };
                await _comboSubjectRepository.Insert(comboSubject);
                await _unitOfWork.SaveChangeAsync();
                comboSubject = await _comboSubjectRepository.GetById(comboSubject.ComboSubjectId);
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = comboSubject;
                dto.Message = "Add Subjects To Tool successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while Add Subjects To Tool: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeleteAllCombosFromSubject(Guid subjectId)
        {
            var dto = new Response();
            try
            {
                var subject = await _subjectRepository.GetById(subjectId);
                if (subject == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Subject not found";
                    return dto;
                }

                var comboSubjects = await _comboSubjectRepository.GetAllDataByExpression(
                    filter: x => x.SubjectId == subjectId,
                    pageNumber: 1,
                    pageSize: int.MaxValue,
                    orderBy: null,
                    isAscending: true,
                    includes: null);

                if (comboSubjects == null || !comboSubjects.Items.Any())
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "No combos found for the specified subject.";
                    return dto;
                }

                await _comboSubjectRepository.DeleteRange(comboSubjects.Items);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Message = "All combos have been deleted from the subject.";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting combos from the subject: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeleteAllSubjectsFromCombo(Guid comboId)
        {
            var dto = new Response();
            try
            {
                var combo = await _comboRepository.GetById(comboId);
                if (combo == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Combo not found";
                    return dto;
                }
                // Lấy toàn bộ các Subject liên kết với Combo này
                var comboSubjects = await _comboSubjectRepository.GetAllDataByExpression(
                    filter: x => x.ComboId == comboId,
                    pageNumber: 1,
                    pageSize: int.MaxValue,
                    orderBy: null,
                    isAscending: true,
                    includes: null);
                if (comboSubjects == null || !comboSubjects.Items.Any())
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "No subjects found for the specified combo.";
                    return dto;
                }
                // Xoá toàn bộ
                await _comboSubjectRepository.DeleteRange(comboSubjects.Items);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Message = "All subjects have been deleted from the combo.";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting subjects from the combo: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeleteCombosFromSubject(Guid subjectId, List<Guid> comboIds)
        {
            var dto = new Response();
            try
            {
                var subject = await _subjectRepository.GetById(subjectId);
                if (subject == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Subject not found.";
                    return dto;
                }

                var comboSubjects = await _comboSubjectRepository.GetAllDataByExpression(
                    filter: x => x.SubjectId == subjectId && comboIds.Contains((Guid)x.ComboId),
                    pageNumber: 1,
                    pageSize: int.MaxValue,
                    orderBy: null,
                    isAscending: true,
                    includes: null);

                if (comboSubjects == null || !comboSubjects.Items.Any())
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "No matching combo links found for the subject.";
                    return dto;
                }

                await _comboSubjectRepository.DeleteRange(comboSubjects.Items);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Data = comboSubjects.Items.Count;
                dto.Message = "Combos have been deleted from the subject.";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting combos from the subject: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeleteSubjectsFromCombo(Guid comboId, List<Guid> subjectIds)
        {
            var dto = new Response();
            try
            {
                var combo = await _comboRepository.GetById(comboId);
                if (combo == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Combo not found.";
                    return dto;
                }

                // Lấy danh sách liên kết Combo - Subject theo danh sách subjectIds
                var comboSubjects = await _comboSubjectRepository.GetAllDataByExpression(
                    filter: x => x.ComboId == comboId && subjectIds.Contains((Guid)x.SubjectId),
                    pageNumber: 1,
                    pageSize: int.MaxValue,
                    orderBy: null,
                    isAscending: true,
                    includes: null);

                if (comboSubjects == null || !comboSubjects.Items.Any())
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "No matching subject links found for the combo.";
                    return dto;
                }

                await _comboSubjectRepository.DeleteRange(comboSubjects.Items);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Data = comboSubjects.Items.Count;
                dto.Message = "Subjects have been deleted from the combo.";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting subjects from the combo: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetAllCombosForSubject(Guid subjectId, int pageNumber, int pageSize)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _comboSubjectRepository.GetAllDataByExpression(
                    filter: x => x.SubjectId == subjectId,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: null,
                    isAscending: true,
                    includes: x => x.Combo);
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Combos for Subject retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving Combos for Subject: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetAllSubjectsForCombo(Guid comboId, int pageNumber, int pageSize)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _comboSubjectRepository.GetAllDataByExpression(
                    filter: t => t.ComboId == comboId,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: null,
                    isAscending: true,
                    includes: t => t.Subject);
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Subjects For Combo retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving Subjects For Combo: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetComboSubjectById(Guid ComboSubjectId)
        {
            Response dto = new Response();
            try
            {
                var comboSubject = await _comboSubjectRepository.GetById(ComboSubjectId);
                if (comboSubject == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Combo Subject not found";
                    return dto;
                }
                dto.Data = comboSubject;
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Combo Subject retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving Combo Subject: " + ex.Message;
            }
            return dto;
        }
    }
}
