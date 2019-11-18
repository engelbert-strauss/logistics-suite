#!/bin/sh

CAKE_VERSION="0.33.0"
TOOLS_PATH="./tools"

if hash dotnet-cake 2>/dev/null; then
    dotnet-cake "$@"
else
    mkdir -p $TOOLS_PATH
    dotnet "tool" "install" "Cake.Tool" "--version=$CAKE_VERSION" "--tool-path=$TOOLS_PATH"
    $TOOLS_PATH/dotnet-cake "$@"
fi
