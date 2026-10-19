using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using TARge25shop.Core.Dto;
using TARge25shop.Core.ServiceInterface;
using TARge25shop.SpaceshipTest;
using Xunit;

namespace TARge25Shop.SpaceshipTest
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
