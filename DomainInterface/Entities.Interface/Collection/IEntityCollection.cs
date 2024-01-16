using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainInterface
{

        public interface IEntityCollection<TEntity> : IEnumerable<TEntity>, ICollection<TEntity>
        //where TEntity : class
        //ICollection ,
        {

            TEntity this[int i] { get; set; }
            // bool IsLoaded { get; }
            //void Add(TEntity entity);
            //bool Remove(TEntity entity);


        }
}