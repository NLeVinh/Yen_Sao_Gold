// .eslintrc.base.js
module.exports = {
    parser: '@typescript-eslint/parser',
    extends: [
        'eslint:recommended',
        'plugin:react/recommended',
        'plugin:@typescript-eslint/recommended',
        'prettier',
    ],
    plugins: ['react', '@typescript-eslint'],
    rules: {
        semi: ['error', 'always'],
        quotes: ['error', 'single'],
        'react/react-in-jsx-scope': 'off', // cho Next.js
    },
    settings: {
        react: {
            version: 'detect',
        },
    },
};
