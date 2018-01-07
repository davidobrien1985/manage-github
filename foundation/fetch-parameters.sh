#!/bin/sh

cat <<EOF > parameters.json
{
    "CommitHash": "${CODEBUILD_RESOLVED_SOURCE_VERSION}",
    "PathToArtifact": "${CODEBUILD_RESOLVED_SOURCE_VERSION}/publish.zip",
    "AccountName": "${ACCOUNT_NAME}"
}
EOF