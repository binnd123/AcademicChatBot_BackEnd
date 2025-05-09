﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.SubjectInCurriculum;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;

namespace AcademicChatBot.Service.Implementation
{
    public class SubjectInCurriculumService : ISubjectInCurriculumService
    {
        private readonly IGenericRepository<Subject> _subjectRepository;
        private readonly IGenericRepository<Curriculum> _curriculumRepository;
        private readonly IGenericRepository<SubjectInCurriculum> _subjectInCurriculumRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SubjectInCurriculumService(
            IGenericRepository<Subject> subjectRepository,
            IGenericRepository<Curriculum> curriculumRepository,
            IGenericRepository<SubjectInCurriculum> subjectInCurriculumRepository,
            IUnitOfWork unitOfWork)
        {
            _subjectRepository = subjectRepository;
            _curriculumRepository = curriculumRepository;
            _subjectInCurriculumRepository = subjectInCurriculumRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> GetSubjectInCurriculumById(Guid subjectInCurriculumId)
        {
            Response dto = new Response();
            try
            {
                var subjectInCurriculum = await _subjectInCurriculumRepository.GetById(subjectInCurriculumId);
                if (subjectInCurriculum == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Subject in curriculum not found";
                    return dto;
                }

                dto.Data = subjectInCurriculum;
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Subject in curriculum retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetAllSubjectsForCurriculum(Guid curriculumId, int pageNumber, int pageSize, int semesterNo)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _subjectInCurriculumRepository.GetAllDataByExpression(
                    filter: x => x.CurriculumId == curriculumId && (semesterNo == 0? true : x.SemesterNo == semesterNo),
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: x => x.SemesterNo,
                    isAscending: true,
                    includes: new Expression<Func<SubjectInCurriculum, object>>[]
                {
                    c => c.Curriculum,
                    c => c.Subject
                });

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Subjects for curriculum retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetAllCurriculumsForSubject(Guid subjectId, int pageNumber, int pageSize, int semesterNo)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _subjectInCurriculumRepository.GetAllDataByExpression(
                    filter: x => x.SubjectId == subjectId && semesterNo == 0 ? true : x.SemesterNo == semesterNo,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: x => x.SemesterNo,
                    isAscending: true,
                    includes: new Expression<Func<SubjectInCurriculum, object>>[]
                {
                    c => c.Curriculum,
                    c => c.Subject
                });

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Curriculums for subject retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> AddSubjectsToCurriculum(Guid curriculumId, List<SubjectInCurriculumRequest> requests)
        {
            Response dto = new Response();
            try
            {
                var curriculum = await _curriculumRepository.GetById(curriculumId);
                if (curriculum == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Curriculum not found";
                    return dto;
                }

                var subjectCurriculumList = new List<SubjectInCurriculum>();
                foreach (var request in requests)
                {
                    if (request.SemesterNo < 1 || request.SemesterNo > 9)
                    {
                        dto.IsSucess = false;
                        dto.BusinessCode = BusinessCode.EXCEPTION;
                        dto.Message = "SemesterNo is Invalid. SemesterNo must be from 1 to 9.";
                        return dto;
                    }
                    var subject = await _subjectRepository.GetById(request.SubjectId);
                    if (subject == null) continue;

                    var existing = await _subjectInCurriculumRepository.GetFirstByExpression(
                        x => x.CurriculumId == curriculumId && x.SubjectId == request.SubjectId);
                    if (existing != null) continue;

                    subjectCurriculumList.Add(new SubjectInCurriculum
                    {
                        SubjectInCurriculumId = Guid.NewGuid(),
                        CurriculumId = curriculumId,
                        SubjectId = request.SubjectId,
                        SemesterNo = request.SemesterNo
                    });
                }

                await _subjectInCurriculumRepository.InsertRange(subjectCurriculumList);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = subjectCurriculumList;
                dto.Message = "Subjects added to curriculum successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> AddCurriculumsToSubject(Guid subjectId, List<CurriculumInSubjectRequest> requests)
        {
            Response dto = new Response();
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

                var subjectCurriculumList = new List<SubjectInCurriculum>();
                foreach (var request in requests)
                {
                    if (request.SemesterNo < 1 || request.SemesterNo > 9)
                    {
                        dto.IsSucess = false;
                        dto.BusinessCode = BusinessCode.EXCEPTION;
                        dto.Message = "SemesterNo is Invalid. SemesterNo must be from 1 to 9.";
                        return dto;
                    }
                    var curriculum = await _curriculumRepository.GetById(request.CurriculumId);
                    if (curriculum == null) continue;

                    var existing = await _subjectInCurriculumRepository.GetFirstByExpression(
                        x => x.SubjectId == subjectId && x.CurriculumId == request.CurriculumId);
                    if (existing != null) continue;

                    subjectCurriculumList.Add(new SubjectInCurriculum
                    {
                        SubjectInCurriculumId = Guid.NewGuid(),
                        SubjectId = subjectId,
                        CurriculumId = request.CurriculumId,
                        SemesterNo = request.SemesterNo
                    });
                }

                await _subjectInCurriculumRepository.InsertRange(subjectCurriculumList);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = subjectCurriculumList;
                dto.Message = "Curriculums added to subject successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeleteSubjectsFromCurriculum(Guid curriculumId, List<Guid> subjectIds)
        {
            var dto = new Response();
            try
            {
                var curriculum = await _curriculumRepository.GetById(curriculumId);
                if (curriculum == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Curriculum not found";
                    return dto;
                }

                var subjectsInCurriculum = await _subjectInCurriculumRepository.GetAllDataByExpression(
                    filter: x => x.CurriculumId == curriculumId && subjectIds.Contains((Guid)x.SubjectId),
                    pageNumber: 1,
                    pageSize: int.MaxValue,
                    orderBy: null,
                    isAscending: true,
                    includes: null);

                if (subjectsInCurriculum == null || !subjectsInCurriculum.Items.Any())
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "No subjects found to delete";
                    return dto;
                }

                await _subjectInCurriculumRepository.DeleteRange(subjectsInCurriculum.Items);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Data = subjectsInCurriculum.Items.Count;
                dto.Message = "Subjects deleted from curriculum successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeleteCurriculumsFromSubject(Guid subjectId, List<Guid> curriculumIds)
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

                var curriculumsForSubject = await _subjectInCurriculumRepository.GetAllDataByExpression(
                    filter: x => x.SubjectId == subjectId && curriculumIds.Contains((Guid)x.CurriculumId),
                    pageNumber: 1,
                    pageSize: int.MaxValue,
                    orderBy: null,
                    isAscending: true,
                    includes: null);

                if (curriculumsForSubject == null || !curriculumsForSubject.Items.Any())
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "No curriculums found to delete";
                    return dto;
                }

                await _subjectInCurriculumRepository.DeleteRange(curriculumsForSubject.Items);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Data = curriculumsForSubject.Items.Count;
                dto.Message = "Curriculums deleted from subject successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeleteAllSubjectsFromCurriculum(Guid curriculumId)
        {
            var dto = new Response();
            try
            {
                var curriculum = await _curriculumRepository.GetById(curriculumId);
                if (curriculum == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Curriculum not found";
                    return dto;
                }

                var subjectsInCurriculum = await _subjectInCurriculumRepository.GetAllDataByExpression(
                    filter: x => x.CurriculumId == curriculumId,
                    pageNumber: 1,
                    pageSize: int.MaxValue,
                    orderBy: null,
                    isAscending: true,
                    includes: null);

                if (subjectsInCurriculum == null || !subjectsInCurriculum.Items.Any())
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "No subjects found in curriculum";
                    return dto;
                }

                await _subjectInCurriculumRepository.DeleteRange(subjectsInCurriculum.Items);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Message = "All subjects deleted from curriculum successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeleteAllCurriculumsFromSubject(Guid subjectId)
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

                var curriculumsForSubject = await _subjectInCurriculumRepository.GetAllDataByExpression(
                    filter: x => x.SubjectId == subjectId,
                    pageNumber: 1,
                    pageSize: int.MaxValue,
                    orderBy: null,
                    isAscending: true,
                    includes: null);

                if (curriculumsForSubject == null || !curriculumsForSubject.Items.Any())
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "No curriculums found for subject";
                    return dto;
                }

                await _subjectInCurriculumRepository.DeleteRange(curriculumsForSubject.Items);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Message = "All curriculums deleted from subject successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred: " + ex.Message;
            }
            return dto;
        }
    }
}
