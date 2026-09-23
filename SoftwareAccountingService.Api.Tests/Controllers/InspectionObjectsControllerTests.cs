using Microsoft.AspNetCore.Mvc;
using Moq;
using SoftwareAccountingService.Api.Controllers;
using SoftwareAccountingService.Api.Domain.Enums;
using SoftwareAccountingService.Api.DTO;
using SoftwareAccountingService.Api.Services;
using Xunit;

namespace SoftwareAccountingService.Api.Tests.Controllers
{
    public class InspectionObjectsControllerTests
    {
        [Fact]
        public async Task GetAll_ReturnsOkWithObjects()
        {
            // Arrange
            var serviceMock = new Mock<IInspectionObjectService>();

            var objects = new List<InspectionObjectDto>
            {
                CreateInspectionObject()
            };

            serviceMock
                .Setup(service => service.GetAllAsync(
                    CancellationToken.None,
                    "Альфа",
                    InspectionResult.InProgress,
                    InspectionType.SW))
                .ReturnsAsync(objects);

            var controller = new InspectionObjectsController(
                serviceMock.Object);

            // Act
            ActionResult<List<InspectionObjectDto>> response =
                await controller.GetAll(
                    "Альфа",
                    InspectionResult.InProgress,
                    InspectionType.SW,
                    CancellationToken.None);

            // Assert
            OkObjectResult okResult =
                Assert.IsType<OkObjectResult>(response.Result);

            List<InspectionObjectDto> returnedObjects =
                Assert.IsType<List<InspectionObjectDto>>(okResult.Value);

            Assert.Single(returnedObjects);
            Assert.Equal("Система Альфа", returnedObjects[0].Name);
        }

        [Fact]
        public async Task GetById_WhenObjectExists_ReturnsOk()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            InspectionObjectDto inspectionObject =
                CreateInspectionObject(id);

            var serviceMock = new Mock<IInspectionObjectService>();

            serviceMock
                .Setup(service => service.GetByIdAsync(
                    id,
                    CancellationToken.None))
                .ReturnsAsync(inspectionObject);

            var controller = new InspectionObjectsController(
                serviceMock.Object);

            // Act
            ActionResult<InspectionObjectDto> response =
                await controller.GetById(
                    id,
                    CancellationToken.None);

            // Assert
            OkObjectResult okResult =
                Assert.IsType<OkObjectResult>(response.Result);

            InspectionObjectDto returnedObject =
                Assert.IsType<InspectionObjectDto>(okResult.Value);

            Assert.Equal(id, returnedObject.Id);
        }

        [Fact]
        public async Task GetById_WhenObjectDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var serviceMock = new Mock<IInspectionObjectService>();

            serviceMock
                .Setup(service => service.GetByIdAsync(
                    id,
                    CancellationToken.None))
                .ReturnsAsync((InspectionObjectDto?)null);

            var controller = new InspectionObjectsController(
                serviceMock.Object);

            // Act
            ActionResult<InspectionObjectDto> response =
                await controller.GetById(
                    id,
                    CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundResult>(response.Result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedObject()
        {
            // Arrange
            var createDto = new CreateInspectionObjectDto
            {
                Name = "Система Альфа",
                Version = "2.4.1",
                Type = InspectionType.SW,
                ReceivedDate = new DateTime(2026, 9, 22),
                Note = "Тестовый объект"
            };

            InspectionObjectDto createdObject =
                CreateInspectionObject();

            var serviceMock = new Mock<IInspectionObjectService>();

            serviceMock
                .Setup(service => service.CreateAsync(
                    createDto,
                    CancellationToken.None))
                .ReturnsAsync(createdObject);

            var controller = new InspectionObjectsController(
                serviceMock.Object);

            // Act
            ActionResult<InspectionObjectDto> response =
                await controller.Create(
                    createDto,
                    CancellationToken.None);

            // Assert
            CreatedAtActionResult createdResult =
                Assert.IsType<CreatedAtActionResult>(response.Result);

            Assert.Equal(
                nameof(InspectionObjectsController.GetById),
                createdResult.ActionName);

            Assert.Equal(
                createdObject.Id,
                Assert.IsType<Guid>(createdResult.RouteValues!["id"]));

            Assert.Same(createdObject, createdResult.Value);
        }

        [Fact]
        public async Task UpdateResult_WhenObjectExists_ReturnsOk()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            var updateDto = new UpdateInspectionResultDto
            {
                Result = InspectionResult.Compliant,
                Note = "Проверка завершена"
            };

            InspectionObjectDto updatedObject =
                CreateInspectionObject(id);

            updatedObject.Result = InspectionResult.Compliant;
            updatedObject.Note = "Проверка завершена";

            var serviceMock = new Mock<IInspectionObjectService>();

            serviceMock
                .Setup(service => service.UpdateResultAsync(
                    id,
                    updateDto,
                    CancellationToken.None))
                .ReturnsAsync(updatedObject);

            var controller = new InspectionObjectsController(
                serviceMock.Object);

            // Act
            ActionResult<InspectionObjectDto> response =
                await controller.UpdateResult(
                    id,
                    updateDto,
                    CancellationToken.None);

            // Assert
            OkObjectResult okResult =
                Assert.IsType<OkObjectResult>(response.Result);

            InspectionObjectDto returnedObject =
                Assert.IsType<InspectionObjectDto>(okResult.Value);

            Assert.Equal(
                InspectionResult.Compliant,
                returnedObject.Result);

            Assert.Equal(
                "Проверка завершена",
                returnedObject.Note);
        }

        [Fact]
        public async Task UpdateResult_WhenObjectDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            var updateDto = new UpdateInspectionResultDto
            {
                Result = InspectionResult.NonCompliant,
                Note = "Найдены нарушения"
            };

            var serviceMock = new Mock<IInspectionObjectService>();

            serviceMock
                .Setup(service => service.UpdateResultAsync(
                    id,
                    updateDto,
                    CancellationToken.None))
                .ReturnsAsync((InspectionObjectDto?)null);

            var controller = new InspectionObjectsController(
                serviceMock.Object);

            // Act
            ActionResult<InspectionObjectDto> response =
                await controller.UpdateResult(
                    id,
                    updateDto,
                    CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundResult>(response.Result);
        }

        private static InspectionObjectDto CreateInspectionObject(
            Guid? id = null)
        {
            return new InspectionObjectDto
            {
                Id = id ?? Guid.NewGuid(),
                Name = "Система Альфа",
                Version = "2.4.1",
                Type = InspectionType.SW,
                ReceivedDate = new DateTime(2026, 9, 22),
                Result = InspectionResult.InProgress,
                Note = "Тестовый объект"
            };
        }
    }
}
