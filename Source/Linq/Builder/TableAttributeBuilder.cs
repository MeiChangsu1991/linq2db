﻿using System;
using System.Linq.Expressions;

namespace LinqToDB.Linq.Builder
{
	using LinqToDB.Expressions;

	using SqlQuery;

	class TableAttributeBuilder : MethodCallBuilder
	{
		protected override bool CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			return methodCall.IsQueryable("TableName", "DatabaseName", "OwnerName", "TableTempType");
		}

		protected override IBuildContext BuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			var sequence = builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[0]));

			var table = (TableBuilder.TableContext)sequence;

			if (methodCall.Method.Name == "TableTempType")
			{
				table.SqlTable.SqlTableTempType = (SqlTableTempType)((ConstantExpression)methodCall.Arguments[1]).Value;
			}
			else
			{
				var value = (string)((ConstantExpression)methodCall.Arguments[1]).Value;

				switch (methodCall.Method.Name)
				{
					case "TableName"    : table.SqlTable.PhysicalName = value; break;
					case "DatabaseName" : table.SqlTable.Database     = value; break;
					case "OwnerName"    : table.SqlTable.Owner        = value; break;
				}
			}

			return sequence;
		}

		protected override SequenceConvertInfo Convert(
			ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo, ParameterExpression param)
		{
			return null;
		}
	}
}