using TARge25shop.Core.Dto;
using TARge25shop.Core.ServiceInterface;
using Xunit;

namespace TARge25Shop.SpaceshipTest
{
    public class KindergartenTest : TestBase
    {
        // Kontrollib, et korrektsete andmetega Kindergarten loomine õnnestub.
        [Fact]
        public async Task Should_AddKindergarten_WhenResultIsReturned()
        {
            // Ülesseade
            var dto = MockKindergartenData();

            // Tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            // Kontroll
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(dto.GroupName, result.GroupName);
            Assert.Equal(dto.KindergartenName, result.KindergartenName);
            Assert.Equal(dto.ChildrenCount, result.ChildrenCount);
            Assert.Equal(dto.TeacherName, result.TeacherName);
        }

        // Kontrollib, et olemasoleva ID järgi leitakse õige Kindergarten.
        [Fact]
        public async Task Should_GetKindergartenByID_WhenGuidIsEqual()
        {
            // Ülesseade
            var dto = MockKindergartenData();
            var createdGroup = await Svc<IKindergartenServices>().Create(dto);

            // Tegevus
            var result = await Svc<IKindergartenServices>()
                .DetailAsync((Guid)createdGroup.Id);

            // Kontroll
            Assert.NotNull(result);
            Assert.Equal(createdGroup.Id, result.Id);
        }

        // Kontrollib, et olematu ID järgi Kindergarten'i ei leita.
        [Fact]
        public async Task ShouldNot_GetKindergartenByID_WhenIDNotEqual()
        {
            // Ülesseade
            var dto = MockKindergartenData();
            var createdGroup = await Svc<IKindergartenServices>().Create(dto);

            Guid wrongGuid = Guid.NewGuid();

            // Tegevus
            var result = await Svc<IKindergartenServices>()
                .DetailAsync(wrongGuid);

            // Kontroll
            Assert.NotEqual(createdGroup.Id, wrongGuid);
            Assert.Null(result);
        }

        // Kontrollib, et kustutamisel tagastatakse kustutatud objekti õige ID.
        [Fact]
        public async Task Should_KindergartenDeletedByID_WhenReturnedResultIsEqual()
        {
            // Ülesseade
            var dto = MockKindergartenData();
            var createdGroup = await Svc<IKindergartenServices>().Create(dto);

            // Tegevus
            var deleteGroup = await Svc<IKindergartenServices>()
                .Delete((Guid)createdGroup.Id);

            // Kontroll
            Assert.NotNull(deleteGroup);
            Assert.Equal(createdGroup.Id, deleteGroup.Id);
        }

        // Kontrollib, et ühe Kindergarten'i kustutamine ei kustuta teist.
        [Fact]
        public async Task ShouldNot_DeleteKindergartenByID_WhenDidNotDeleteKindergarten()
        {
            // Ülesseade
            var dto1 = MockKindergartenData(false);
            var dto2 = MockKindergartenData(true);

            var kindergarten1 = await Svc<IKindergartenServices>().Create(dto1);
            var kindergarten2 = await Svc<IKindergartenServices>().Create(dto2);

            // Tegevus
            var result = await Svc<IKindergartenServices>()
                .Delete((Guid)kindergarten2.Id);

            // Kontroll
            Assert.NotNull(result);
            Assert.NotEqual(kindergarten1.Id, result.Id);

            var firstKindergarten = await Svc<IKindergartenServices>()
                .DetailAsync((Guid)kindergarten1.Id);

            Assert.NotNull(firstKindergarten);
            Assert.Equal(kindergarten1.Id, firstKindergarten.Id);
        }

        // Kontrollib, et Kindergarten andmete uuendamisel
        // jääb ID samaks ja muud andmed muutuvad õigeks.
        [Fact]
        public async Task Should_UpdateKindergartenByID_WhenUpdatingData()
        {
            // Ülesseade
            var dto = MockKindergartenData();
            var createdGroup = await Svc<IKindergartenServices>().Create(dto);

            var updateDto = new KindergartenDto
            {
                Id = createdGroup.Id,
                GroupName = "Karupojad",
                KindergartenName = "Tallinna Lasteaed",
                ChildrenCount = 12,
                TeacherName = "Õpetaja Tamm",
                CreatedAt = createdGroup.CreatedAt,
                UpdatedAt = DateTime.UtcNow
            };

            // Tegevus
            var result = await Svc<IKindergartenServices>()
                .Update(updateDto);

            // Kontroll
            Assert.NotNull(result);
            Assert.Equal(createdGroup.Id, result.Id);
            Assert.Equal("Karupojad", result.GroupName);
            Assert.Equal("Tallinna Lasteaed", result.KindergartenName);
            Assert.Equal(12, result.ChildrenCount);
            Assert.Equal("Õpetaja Tamm", result.TeacherName);
        }

        // Kontrollib, et olematu ID-ga Kindergarten'i ei saa uuendada.
        [Fact]
        public async Task ShouldNot_UpdateKindergartenByID_WhenNoDataIsUpdated()
        {
            // Ülesseade
            var dto = MockKindergartenData();
            var createdGroup = await Svc<IKindergartenServices>().Create(dto);
            var nullDto = MockKindergartenNullData();

            // Tegevus
            var result = await Svc<IKindergartenServices>()
                .Update(nullDto);

            // Kontroll
            Assert.Null(result);

            // Kontrollime, et olemasolev objekt on endiselt alles.
            var existingGroup = await Svc<IKindergartenServices>()
                .DetailAsync((Guid)createdGroup.Id);

            Assert.NotNull(existingGroup);
            Assert.Equal(createdGroup.Id, existingGroup.Id);
            Assert.Equal(createdGroup.GroupName, existingGroup.GroupName);
            Assert.Equal(createdGroup.TeacherName, existingGroup.TeacherName);
            Assert.Equal(createdGroup.ChildrenCount, existingGroup.ChildrenCount);
        }

        // Kontrollib, et negatiivne laste arv muudetakse nulliks.
        [Fact]
        public async Task ShouldNot_CreateKindergartenWithNegativeChildrenCount_WhenCountIsNegative()
        {
            // Ülesseade
            var dto = MockKindergartenData();
            dto.ChildrenCount = -5;

            // Tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            // Kontroll
            Assert.NotNull(result);
            Assert.True(result.ChildrenCount >= 0);
        }

        // Kontrollib, et ilma õpetaja nimeta Kindergarten'i ei looda.
        [Fact]
        public async Task ShouldNot_CreateKindergarten_WhenTeacherNameIsEmpty()
        {
            // Ülesseade
            var dto = MockKindergartenData();
            dto.TeacherName = "";

            // Tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            // Kontroll
            Assert.Null(result);
        }

        // Kontrollib, et pärast kustutamist ei ole objekti enam võimalik leida.
        [Fact]
        public async Task Should_RemoveKindergartenFromDatabase_WhenKindergartenIsDeleted()
        {
            // Ülesseade
            var dto = MockKindergartenData();
            var createdGroup = await Svc<IKindergartenServices>().Create(dto);

            // Tegevus
            await Svc<IKindergartenServices>()
                .Delete((Guid)createdGroup.Id);

            var result = await Svc<IKindergartenServices>()
                .DetailAsync((Guid)createdGroup.Id);

            // Kontroll
            Assert.Null(result);
        }

        // Kontrollib, et olematu ID puhul tagastab DetailAsync null.
        [Fact]
        public async Task Should_ReturnNull_WhenKindergartenDoesNotExist()
        {
            // Ülesseade
            Guid nonExistingGuid = Guid.NewGuid();

            // Tegevus
            var result = await Svc<IKindergartenServices>()
                .DetailAsync(nonExistingGuid);

            // Kontroll
            Assert.Null(result);
        }

        // Kontrollib, et olematu ID kustutamisel tagastatakse null.
        [Fact]
        public async Task Should_ReturnNull_WhenDeletingNonExistingKindergarten()
        {
            // Ülesseade
            Guid nonExistingGuid = Guid.NewGuid();

            // Tegevus
            var result = await Svc<IKindergartenServices>()
                .Delete(nonExistingGuid);

            // Kontroll
            Assert.Null(result);
        }

        // Kontrollib, et tühja rühma nimega Kindergarten'i ei looda.
        [Fact]
        public async Task ShouldNot_CreateKindergarten_WhenGroupNameIsEmpty()
        {
            // Ülesseade
            var dto = MockKindergartenData();
            dto.GroupName = "";

            // Tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            // Kontroll
            Assert.Null(result);
        }

        // Kontrollib, et laste arvu uuendamisel salvestatakse uus väärtus.
        [Fact]
        public async Task Should_UpdateChildrenCount_WhenDataIsCorrect()
        {
            // Ülesseade
            var dto = MockKindergartenData();
            var createdGroup = await Svc<IKindergartenServices>().Create(dto);

            var updateDto = new KindergartenDto
            {
                Id = createdGroup.Id,
                GroupName = createdGroup.GroupName,
                KindergartenName = createdGroup.KindergartenName,
                ChildrenCount = 25,
                TeacherName = createdGroup.TeacherName,
                CreatedAt = createdGroup.CreatedAt,
                UpdatedAt = DateTime.UtcNow
            };

            // Tegevus
            var result = await Svc<IKindergartenServices>()
                .Update(updateDto);

            // Kontroll
            Assert.NotNull(result);
            Assert.Equal(25, result.ChildrenCount);
        }

        // Kontrollib, et kaks Kindergarten'i saavad erinevad ID-d.
        [Fact]
        public async Task Should_GenerateUniqueIds_WhenTwoKindergartensAreCreated()
        {
            // Ülesseade
            var dto1 = MockKindergartenData(false);
            var dto2 = MockKindergartenData(true);

            // Tegevus
            var result1 = await Svc<IKindergartenServices>().Create(dto1);
            var result2 = await Svc<IKindergartenServices>().Create(dto2);

            // Kontroll
            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.NotEqual(result1.Id, result2.Id);
        }

        // Abimeetod vigaste või puuduvate andmete jaoks.
        private KindergartenDto MockKindergartenNullData()
        {
            return new KindergartenDto
            {
                Id = null,
                GroupName = "",
                KindergartenName = "",
                ChildrenCount = 0,
                TeacherName = "",
                CreatedAt = DateTime.MinValue,
                UpdatedAt = DateTime.MinValue
            };
        }

        // Abimeetod korrektsete testandmete loomiseks.
        private KindergartenDto MockKindergartenData(bool isOneOrTwo = false)
        {
            if (!isOneOrTwo)
            {
                return new KindergartenDto
                {
                    GroupName = "Päikesekiir",
                    KindergartenName = "Lasteaia tee 5, Tallinn",
                    ChildrenCount = 18,
                    TeacherName = "Maiu",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
            }

            return new KindergartenDto
            {
                GroupName = "Sipsik",
                KindergartenName = "Kooli tänav 12, Tartu",
                ChildrenCount = 22,
                TeacherName = "Maiu",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }
    }
}