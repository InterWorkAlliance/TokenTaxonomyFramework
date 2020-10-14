# TTF Web UI

## Build

Generate the necessary Typescript bindings by running `artifacts/build-model.sh`

The Typescript files are generated under `tools/TaxonomyObjectModel/out/ts`.

Install yarn then run the following:
```
yarn install
yarn build
```

Run the application in a development setting with:
```
yarn start
```

You can produce the bundled Javascript with:
```
yarn webpack
```