namespace EstimatorX.Domain;

/// <summary>
/// Well-known query pipeline names used as the <c>FilterName</c> on entity commands and queries.
/// </summary>
/// <remarks>
/// <para>
/// When a request specifies a <c>FilterName</c>, the query pipeline resolves only the
/// <c>IQueryModifier&lt;TEntity&gt;</c> instances registered as keyed services under that name.
/// The default (non-keyed) modifiers, such as the user and organization security filters, are not applied.
/// </para>
/// <para>
/// Pipeline names must only be set by trusted server-side code. Never allow a client supplied value
/// to flow into <c>FilterName</c>, as it would bypass the default security filters.
/// </para>
/// </remarks>
public static class PipelineNames
{
    /// <summary>
    /// The system pipeline used by background tasks that require unrestricted access to all data.
    /// </summary>
    /// <remarks>
    /// No query modifiers are registered for this pipeline, so queries run without any user or
    /// organization filtering. Do not register keyed modifiers under this name unless system-level
    /// filtering is intentionally required.
    /// </remarks>
    /// <example>
    /// <code>
    /// var query = new EntityPagedQuery&lt;ProjectReadModel&gt;(principal, entityQuery, filterName: PipelineNames.System);
    /// </code>
    /// </example>
    public const string System = "System";
}
