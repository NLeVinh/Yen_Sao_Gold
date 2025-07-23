'use client';
import { createTheme } from '@mui/material/styles';
import { Roboto } from 'next/font/google';

const roboto = Roboto({
    weight: ['300', '400', '500', '700'],
    subsets: ['latin'],
    display: 'swap',
});

const theme = createTheme({
    colorSchemes: { light: true, dark: true },
    cssVariables: {
        colorSchemeSelector: 'class',
    },
    typography: {
        fontFamily: roboto.style.fontFamily,
    },
    components: {
        MuiAlert: {
            styleOverrides: {
                root: {
                    variants: [
                        {
                            props: { severity: 'info' },
                            style: {
                                backgroundColor: '#60a5fa',
                            },
                        },
                    ],
                },
            },
        },
        MuiTextField: {
            defaultProps: {
                variant: 'outlined',
                fullWidth: true,
            },
        },
        MuiOutlinedInput: {
            styleOverrides: {
                root: {
                    height: '32px',
                    borderRadius: '3px',
                    fontSize: '0.95rem',
                    // minHeight: '48px',
                },
            },
        },
        MuiInputBase: {
            styleOverrides: {
                input: {
                    padding: '14px',
                    boxSizing: 'border-box',
                },
            },
        },
        MuiInputLabel: {
            styleOverrides: {
                root: {
                    fontSize: '0.85rem',
                    transform: 'translate(14px, 12px) scale(1)',
                    '&.Mui-focused': {
                        color: '#080b0eff',
                    },
                },
            },
        },
        MuiFormHelperText: {
            styleOverrides: {
                root: {
                    fontSize: '0.75rem',
                    color: '#d32f2f', // Màu mặc định khi lỗi
                    marginTop: '4px',
                    marginLeft: '2px',
                },
            },
        },
        MuiButton: {
            styleOverrides: {
                root: {
                    backgroundColor: '#000000',
                    color: '#ffffff',
                    '&:hover': {
                        backgroundColor: '#333333',
                    },
                },
            },
        },
    },
});

export default theme;
