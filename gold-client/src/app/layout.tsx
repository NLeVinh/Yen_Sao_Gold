import * as React from 'react';
import { AppRouterCacheProvider } from '@mui/material-nextjs/v15-appRouter';
import { ThemeProvider } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import theme from '@/theme';
import InitColorSchemeScript from '@mui/material/InitColorSchemeScript';
import { ReduxProvider } from './ReduxProvider';

import '@/styles/main.scss';
import Header from '@/components/shared/Header';

export default function RootLayout(props: { children: React.ReactNode }) {
    return (
        <html lang="en" suppressHydrationWarning>
            <body>
                <InitColorSchemeScript attribute="class" />
                <AppRouterCacheProvider options={{ enableCssLayer: true }}>
                    <ReduxProvider>
                        <ThemeProvider theme={theme}>
                            <Header />
                            <CssBaseline />
                            {props.children}
                        </ThemeProvider>
                    </ReduxProvider>
                </AppRouterCacheProvider>
            </body>
        </html>
    );
}
