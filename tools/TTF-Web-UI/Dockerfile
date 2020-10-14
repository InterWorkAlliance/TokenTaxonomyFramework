FROM node:10.15.1-stretch as built

ENV NODE_ENV production
ENV SKIP_PREFLIGHT_CHECK true

WORKDIR /build

COPY package.json tsconfig.json webpack.config.js yarn.lock ./
COPY src/ src/
COPY public/ public/

RUN yarn install
RUN yarn add --dev @typescript-eslint/eslint-plugin node-sass
RUN yarn add @typescript-eslint/eslint-plugin node-sass
RUN yarn build

FROM nginx:alpine as final
COPY --from=built /build/build /usr/share/nginx/html
COPY nginx/default.conf.tmpl /etc/nginx/conf.d/default.conf.tmpl
COPY nginx/start.sh /usr/start.sh

CMD /usr/start.sh