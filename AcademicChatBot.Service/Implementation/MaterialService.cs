using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.Material;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;

namespace AcademicChatBot.Service.Implementation
{
    public class MaterialService : IMaterialService
    {
        private readonly IGenericRepository<Material> _materialRepository;
        private readonly IGenericRepository<Subject> _subjectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MaterialService(IGenericRepository<Material> materialRepository, IGenericRepository<Subject> subjectRepository, IUnitOfWork unitOfWork)
        {
            _materialRepository = materialRepository;
            _subjectRepository = subjectRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> CreateMaterial(CreateMaterialRequest request)
        {
            Response dto = new Response();
            try
            {
                var materialE = await _materialRepository.GetFirstByExpression(x => x.MaterialCode == request.MaterialCode);
                if (materialE != null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.EXCEPTION;
                    dto.Message = "Material is Existed!";
                    return dto;
                }

                var subject = await _subjectRepository.GetById(request.SubjectId);
                if (subject == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Subject not found";
                    return dto;
                }

                var material = new Material
                {
                    MaterialId = Guid.NewGuid(),
                    MaterialCode = request.MaterialCode,
                    MaterialName = request.MaterialName,
                    MaterialDescription = request.MaterialDescription,
                    Author = request.Author,
                    Publisher = request.Publisher,
                    PublishedDate = request.PublishedDate,
                    Edition = request.Edition,
                    ISBN = request.ISBN,
                    IsMainMaterial = request.IsMainMaterial,
                    IsHardCopy = request.IsHardCopy,
                    IsOnline = request.IsOnline,
                    Note = request.Note,
                    SubjectId = request.SubjectId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                await _materialRepository.Insert(material);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = material;
                dto.Message = "Material created successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while creating the material: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeleteMaterial(Guid materialId)
        {
            Response dto = new Response();
            try
            {
                var material = await _materialRepository.GetById(materialId);
                if (material == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Material not found";
                    return dto;
                }
                material.IsDeleted = true;
                material.DeletedAt = DateTime.Now;
                await _materialRepository.Update(material);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Data = material;
                dto.Message = "Material deleted successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting the material: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetAllMaterials(int pageNumber, int pageSize, string search, SortBy sortBy, SortType sortType, bool isDeleted)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _materialRepository.GetAllDataByExpression(
                    filter: m => (m.MaterialCode.ToLower().Contains(search.ToLower())
                    || m.MaterialName.ToLower().Contains(search.ToLower())
                    || m.MaterialDescription.ToLower().Contains(search.ToLower()))
                    && m.IsDeleted == isDeleted,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: m => sortBy == SortBy.Default ? null : sortBy == SortBy.Name ? m.MaterialName : m.MaterialCode,
                    isAscending: sortType == SortType.Ascending,
                    includes: m => m.Subject);
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Materials retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving materials: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetMaterialById(Guid materialId)
        {
            Response dto = new Response();
            try
            {
                var material = await _materialRepository.GetById(materialId);
                if (material == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Material not found";
                    return dto;
                }
                dto.Data = material;
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Material retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving the material: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> UpdateMaterial(Guid materialId, UpdateMaterialRequest request)
        {
            Response dto = new Response();
            try
            {
                if (request.SubjectId != null)
                {
                    var subject = await _subjectRepository.GetById(request.SubjectId.Value);
                    if (subject == null)
                    {
                        dto.IsSucess = false;
                        dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                        dto.Message = "Subject not found";
                        return dto;
                    }
                }

                var material = await _materialRepository.GetById(materialId);
                if (material == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Material not found";
                    return dto;
                }

                if (!string.IsNullOrEmpty(request.MaterialCode)) material.MaterialCode = request.MaterialCode;
                if (!string.IsNullOrEmpty(request.MaterialName)) material.MaterialName = request.MaterialName;
                if (!string.IsNullOrEmpty(request.MaterialDescription)) material.MaterialDescription = request.MaterialDescription;
                if (!string.IsNullOrEmpty(request.Author)) material.Author = request.Author;
                if (!string.IsNullOrEmpty(request.Publisher)) material.Publisher = request.Publisher;
                if (!string.IsNullOrEmpty(request.Edition)) material.Edition = request.Edition;
                if (!string.IsNullOrEmpty(request.ISBN)) material.ISBN = request.ISBN;
                if (!string.IsNullOrEmpty(request.Note)) material.Note = request.Note;
                material.PublishedDate = request.PublishedDate ?? material.PublishedDate;
                material.IsMainMaterial = request.IsMainMaterial ?? material.IsMainMaterial;
                material.IsHardCopy = request.IsHardCopy ?? material.IsHardCopy;
                material.IsOnline = request.IsOnline ?? material.IsOnline;
                material.SubjectId = request.SubjectId ?? material.SubjectId;
                material.UpdatedAt = DateTime.Now;

                await _materialRepository.Update(material);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.UPDATE_SUCCESSFULLY;
                dto.Data = material;
                dto.Message = "Material updated successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while updating the material: " + ex.Message;
            }
            return dto;
        }
    }
}
