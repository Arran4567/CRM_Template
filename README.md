# Bicks
Management system for Bicks Meat & Poultry

## Updating Models
Whenever you update / add a model the required database changes can be applied by:

1. Make your changes to the models
2. Add your model to the `ApplicationDbContext.cs`
3. Opening the package manager console in Visual Studio `Tools > NuGet Package Manager > Package Manager Console`
4. In the package manager console run `Add-Migration [Name of Migration]` where `[Name of Migration]` is something MEANINGFUL
5. Run the application and the migrations are applied automagically

## Adding a WorkUnit to your Area
Don't inject the context into a controller. Create a WorkUnit that provides access to data respoitories and methods for CRUD-ing data. We will inject the context into the WorkUnit and the WorkUnit into the controller.

1. Create the WorkUnit class under `[YOUR_AREA]/Data/DAL`
	The class should look like the following
	```
	public class ExampleWorkUnit : IDisposable
    {
        private ApplicationDbContext _context;
        private GenericRepository<ExampleObject> exampleObjectRepository;

        public GenericRepository<ExampleObject> Example
        {
            get
            {
                if (exampleObjectRepository == null)
                {
                    exampleObjectRepository = new GenericRepository<ExampleObject>(_context);
                }
                return exampleObjectRepository;
            }
        }

         public ExampleWorkUnit(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
	```
    You can then add `GenericRepository`'s for each database table you want to access
2. Open `/Data/DAL/DependencyInjection.cs` and add a line in `AddWorkUnits` of the following nature: `services.AddTransient<[YOUR_WORK_UNIT_CLASS_NAME]>();`. This will allow ASP.NET to inject it into your controller and to inject the context into the WorkUnit
3. Open your controller and add `[YOUR_WORK_UNIT_CLASS_NAME]` as a parameter of the construtor, so ASP.NET injects it. Save it to a `private readonly` class attribute
