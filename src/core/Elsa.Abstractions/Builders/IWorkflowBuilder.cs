using System;
using Elsa.Models;
using Elsa.Services;
using Elsa.Services.Models;

namespace Elsa.Builders
{
    public interface IWorkflowBuilder : IBuilder
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        int Version { get; }
        WorkflowPersistenceBehavior PersistenceBehavior { get; }
        bool DeleteCompletedInstances { get; }
        IServiceProvider ServiceProvider { get; }
        IWorkflowBuilder WithId(string value);
        IWorkflowBuilder WithName(string value);
        IWorkflowBuilder WithDescription(string value);
        IWorkflowBuilder WithVersion(int value);
        IWorkflowBuilder AsSingleton();
        IWorkflowBuilder AsTransient();
        IWorkflowBuilder WithDeleteCompletedInstances(bool value);
        IWorkflowBuilder WithPersistenceBehavior(WorkflowPersistenceBehavior value);
        T New<T>(Action<T>? setup = default, Action<IActivityBuilder>? branch = default) where T : class, IActivity;
        T New<T>(T activity, Action<IActivityBuilder>? branch = default) where T : class, IActivity;
        IActivityBuilder StartWith<T>(Action<T>? setup = default, Action<IActivityBuilder>? branch = default) where T : class, IActivity;
        IActivityBuilder StartWith<T>(T activity, Action<IActivityBuilder>? branch = default) where T : class, IActivity;
        IActivityBuilder Add<T>(Action<T>? setup = default, Action<IActivityBuilder>? branch = default) where T : class, IActivity;
        IActivityBuilder Add<T>(T activity, Action<IActivityBuilder>? branch = default) where T : class, IActivity;
        IConnectionBuilder Connect(IActivityBuilder source, IActivityBuilder target, string outcome = OutcomeNames.Done);
        IConnectionBuilder Connect(Func<IActivityBuilder> source, Func<IActivityBuilder> target, string outcome = OutcomeNames.Done);
        Workflow Build();
        Workflow Build(IWorkflow workflow);
        Workflow Build(Type workflowType);
        Workflow Build<T>() where T:IWorkflow;
    }
}