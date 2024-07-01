using System;
using Repositories.Dto;
using Repositories.Tests.Fakes;
using Xunit;

namespace Repositories.Tests
{
    // public class TestRepository
    // {
    //     private readonly IRepository<UserEntity, int> userRepo;

    //     public TestRepository(){
    //         var context = new FakeDbScheme();
    //         context.Set<UserEntity>().Add(new UserEntity { Id = 1});
            
    //         userRepo = new Repository<UserEntity>(context);
    //     }
    //     [Fact]
    //     public void FindByKey_Should_Returns_Entity()
    //     {
    //         var results = userRepo.FindByKey(1);
    //         Assert.True(results != null && results.Id == 1 );
    //     }
    // }
}
