﻿using System;
using System.Collections.Generic;

namespace Foxy.Params.SourceGenerator.Data;

internal class FailedParamsCandidate : ParamsCandidate, IEquatable<FailedParamsCandidate?>
{
    public override bool HasErrors => true;

    public required List<DiagnosticInfo> Diagnostics { get; init; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as FailedParamsCandidate);
    }

    public bool Equals(FailedParamsCandidate? other)
    {
        return other is not null &&
               EqualityComparer<List<DiagnosticInfo>>.Default.Equals(Diagnostics, other.Diagnostics);
    }

    public override int GetHashCode()
    {
        return 244270639 + EqualityComparer<List<DiagnosticInfo>>.Default.GetHashCode(Diagnostics);
    }
}

