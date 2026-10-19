using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using TARge25shop.Core.Dto;
using TARge25shop.Core.ServiceInterface;
using TARge25shop.SpaceshipTest;
using Xunit;

namespace TARge25Shop.KindergartenTest
{
    public class KindergartenTest : TestBase
    {
        // Selles testis kontrollitakse, et korrektsete andmete
        // edastamisel lasteaia loomise teenusele peaks tagastatav
        // tulemus olema edukas ning andmebaasi loodud objekt ei tohi olla tühi.


        // Test 1: Kontrollib, kas uue lasteaia andmete edukal lisamisel teenuse kaudu
        // tagastatakse loodud objekt ja see ei ole null. See tagab, et andmete 
        // salvestamise funktsionaalsus käivitub ja tagastab tulemuse.
        [Fact]
        public async Task Should_AddKindergarten_WhenResultIsReturned()
        {
            //ülesseade
            var dto = MockKindergartenData(false);
            //tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            //kontroll
            Assert.NotNull(result);
        }
        // 2. Kontrollime, et vale ID-ga lasteaeda ei leita.
        [Fact]
        public async Task ShouldNot_GetKindergartenByID_WhenIDNotEqual()
        {
            KindergartenDto dto = MockKindergartenData();
            var created = await Svc<IKindergartenServices>().Create(dto);

            Guid wrongGuid = Guid.NewGuid();

            var result = await Svc<IKindergartenServices>()
                .DetailAsync(wrongGuid);

            Assert.NotEqual(wrongGuid, created.Id);
            Assert.Null(result);
        }

        // 3. Kontrollime, et õige ID-ga leitakse õige lasteaed.
        [Fact]
        public async Task Should_GetKindergartenByID_WhenGuidIsEqual()
        {
            KindergartenDto dto = MockKindergartenData();
            var created = await Svc<IKindergartenServices>().Create(dto);

            var result = await Svc<IKindergartenServices>()
                .DetailAsync((Guid)created.Id);

            Assert.NotNull(result);
            Assert.Equal(created.Id, result.Id);
        }

        // 4. Kontrollime, et lasteaia kustutamisel
        // tagastatakse kustutatud lasteaia ID.
        [Fact]
        public async Task Should_KindergartenDeletedByID_WhenReturnedResultIsEqual()
        {
            KindergartenDto dto = MockKindergartenData();

            var created = await Svc<IKindergartenServices>().Create(dto);
            var deleted = await Svc<IKindergartenServices>()
                .Delete((Guid)created.Id);

            Assert.Equal(created.Id, deleted.Id);
        }

        // 5. Kontrollime, et teise lasteaia kustutamisel
        // ei kustutata esimest lasteaeda.
        [Fact]
        public async Task ShouldNot_DeleteKindergartenByID_WhenDeletingAnotherKindergarten()
        {
            var dto1 = MockKindergartenData(false);
            var dto2 = MockKindergartenData(true);

            var kindergarten1 =
                await Svc<IKindergartenServices>().Create(dto1);

            var kindergarten2 =
                await Svc<IKindergartenServices>().Create(dto2);

            var result = await Svc<IKindergartenServices>()
                .Delete((Guid)kindergarten2.Id);

            Assert.NotEqual(kindergarten1.Id, result.Id);
        }
        // 6. Kontrollime, et lasteaia andmeid saab uuendada.
        [Fact]
        public async Task Should_UpdateKindergartenByID_WhenUpdatingData()
        {
            // ülesseade
            KindergartenDto dto = MockKindergartenData();

            var created = await Svc<IKindergartenServices>().Create(dto);

            dto.Id = created.Id;
            dto.GroupName = "Mesimummud";
            dto.KindergartenName = "Tallinna Lasteaed";
            dto.ChildrenCount = 25;
            dto.TeacherName = "Mari";

            // tegevus
            var result = await Svc<IKindergartenServices>().Update(dto);

            // kontroll
            Assert.NotNull(result);
            Assert.Equal(dto.Id, result.Id);
            Assert.Equal("Mesimummud", result.GroupName);
            Assert.Equal("Tallinna Lasteaed", result.KindergartenName);
            Assert.Equal(25, result.ChildrenCount);
            Assert.Equal("Mari", result.TeacherName);
        }

        // 7. Kontrollime, et tühjade andmetega uuendamine
        // ei asendaks olemasolevat lasteaeda.
        [Fact]
        public async Task ShouldNot_UpdateKindergartenByID_WhenNoDataIsUpdated()
        {
            KindergartenDto dto = MockKindergartenData();

            var created = await Svc<IKindergartenServices>().Create(dto);
            var nullDto = MockKindergartenNullData();

            var result = await Svc<IKindergartenServices>().Update(nullDto);

            // KONTROLL: Kuna nullDto ID-d pole andmebaasis, peab result olema null
            Assert.Null(result);
        }

        // 8. Kontrollime, et laste arv ei oleks negatiivne.
        [Fact]
        public async Task ShouldNot_CreateKindergartenWithNegativeChildrenCount_WhenChildrenCountIsNegative()
        {
            KindergartenDto dto = MockKindergartenData();
            dto.ChildrenCount = -5;

            var result = await Svc<IKindergartenServices>().Create(dto);

            Assert.NotNull(result);
            Assert.True(result.ChildrenCount >= 0);
        }




        //Test andmed I
        //            I
        //            V

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
                UpdatedAt = DateTime.MinValue,
            };
        }

        /* üleval testid, all abimeetodid */

        private KindergartenDto MockKindergartenData(bool isOneOrTwo = false)
        {
            if (isOneOrTwo == false)
            {
                return new KindergartenDto
                {
                    GroupName = "Päikesekiir",
                    KindergartenName = "Lasteaia tee 5, Tallinn",
                    ChildrenCount = 18,
                    TeacherName = "Maiu",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };
            }
            else
            {
                return new KindergartenDto
                {
                    GroupName = "Sipsik",
                    KindergartenName = "Kooli tänav 12, Tartu",
                    ChildrenCount = 22,
                    TeacherName = "Maiu",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };
            }
        }
    }
}
