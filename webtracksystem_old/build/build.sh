#!/usr/bin/env bash

export ARTIFACTS="artifacts"
export WEB_CONTENT="content"
export API_CONTENT="api"

#build web
cd ../web
#npm i
#npm test
npm run build
cp -r ./$WEB_CONTENT ../build/$ARTIFACTS/
# build api
cd ../
#dotnet restore api
dotnet publish api -c Release --force -o ../build/$ARTIFACTS/$API_CONTENT