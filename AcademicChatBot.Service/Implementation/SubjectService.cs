using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.DTOs;
using AcademicChatBot.Common.DTOs.BussinessCode;
using AcademicChatBot.Common.DTOs.Subjects;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Implementation;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;
using Azure.Core;

namespace AcademicChatBot.Service.Implementation
{
    public class SubjectService : ISubjectService
    {
        private readonly IGenericRepository<Subject> _subjectRepository;
        private readonly IGenericRepository<Curriculum> _curriculumRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SubjectService(IGenericRepository<Subject> subjectRepository, IGenericRepository<Curriculum> curriculumRepository, IUnitOfWork unitOfWork)
        {
            _subjectRepository = subjectRepository;
            _curriculumRepository = curriculumRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> CreateSubject(CreateSubjectRequest request)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                if (request.CurriculumId != null)
                {
                    var curriculum = await _curriculumRepository.GetById(request.CurriculumId);
                    if (curriculum == null)
                    {
                        dto.IsSucess = false;
                        dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                        dto.Message = "Curriculum not found";
                        return dto;
                    }
                }
                var subject = new Subject
                {
                    SubjectId = Guid.NewGuid(),
                    SubjectCode = request.SubjectCode,
                    IsApproved = request.IsApproved,
                    IsDeleted = false,
                    ApprovedDate = request.ApprovedDate,
                    IsActive = request.IsActive,
                    DecisionNo = request.DecisionNo,
                    NoCredit = request.NoCredit,
                    SubjectName = request.SubjectName,
                    CurriculumId = request.CurriculumId,
                };
                await _subjectRepository.Insert(subject);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = subject;
                dto.Message = "Subject created successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while creating the subject: " + ex.Message;
            }
            return dto;
        }

        public async Task<ResponseDTO> DeleteSubject(Guid SubjectId)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var subject = await _subjectRepository.GetById(SubjectId);
                if (subject == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Subject not found";
                    return dto;
                }
                subject.IsDeleted = true;
                await _subjectRepository.Update(subject);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Data = subject;
                dto.Message = "Subject deleted successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting the subject: " + ex.Message;
            }
            return dto;
        }

        public async Task<ResponseDTO> GetAllSubjects(int pageNumber, int pageSize, string search)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                dto.Data = await _subjectRepository.GetAllDataByExpression(
                    filter: s => (s.SubjectCode.ToLower().Contains(search) && s.IsActive && s.IsApproved && !s.IsDeleted), 
                    pageNumber: pageNumber, 
                    pageSize: pageSize, 
                    orderBy: s => s.SubjectCode, 
                    isAscending: true, 
                    includes: s => s.Curriculum);
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Subjects retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving subjects: " + ex.Message;
            }
            return dto;
        }

        public async Task<ResponseDTO> GetSubjectById(Guid subjectId)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                dto.Data = await _subjectRepository.GetById(subjectId);
                if (dto.Data == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Subject not found";
                    return dto;
                }
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Subject retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving the subject: " + ex.Message;
            }
            return dto;
        }

        public async Task<ResponseDTO> UpdateSubject(Guid SubjectId, UpdateSubjectRequest request)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                if (request.CurriculumId != null)
                {
                    var curriculum = await _curriculumRepository.GetById(request.CurriculumId);
                    if (curriculum == null)
                    {
                        dto.IsSucess = false;
                        dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                        dto.Message = "Curriculum not found";
                        return dto;
                    }
                }
                var subject = await _subjectRepository.GetById(SubjectId);
                if (subject == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Subject not found";
                    return dto;
                }
                if (!string.IsNullOrEmpty(request.SubjectName)) subject.SubjectName = request.SubjectName;

                if (!string.IsNullOrEmpty(request.SubjectCode)) subject.SubjectCode = request.SubjectCode;

                if (!string.IsNullOrEmpty(request.DecisionNo)) subject.DecisionNo = request.DecisionNo;

                if (request.ApprovedDate != null) subject.ApprovedDate = request.ApprovedDate;

                subject.IsActive = request.IsActive;
                subject.IsApproved = request.IsApproved;
                subject.IsActive = request.IsActive;
                subject.CurriculumId = request.CurriculumId;
                await _subjectRepository.Update(subject);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.UPDATE_SUCCESSFULLY;
                dto.Data = subject;
                dto.Message = "Subject updated successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while updating the subject: " + ex.Message;
            }
            return dto;
        }
    }
}
