'use client';

import React from 'react';
import {
    AppBar,
    Toolbar,
    Typography,
    Button,
    Box,
    Container,
    IconButton,
    useScrollTrigger,
    CssBaseline,
} from '@mui/material';
import ListIcon from '@mui/icons-material/List';
import Link from 'next/link';

import { ROUTES } from '@/constants/routeConstant';

interface Props {
    window?: () => Window;
    children?: React.ReactElement<{ elevation?: number; className?: string }>;
}

function ElevationScroll(props: Props) {
    const { children, window } = props;
    const trigger = useScrollTrigger({
        disableHysteresis: true,
        threshold: 0,
        target: window ? window() : undefined,
    });

    return children
        ? React.cloneElement(children, {
              elevation: trigger ? 4 : 0,
              className: trigger ? 'sticky' : 'header',
          })
        : null;
}

const Header = (props: Props) => {
    const renderHeaderList = () => {
        return ROUTES.map((route) => (
            <Button
                key={route.id}
                color="inherit"
                component={Link}
                href={route.path}
                className="nav-button"
            >
                {route.label}
            </Button>
        ));
    };

    return (
        <>
            <CssBaseline />
            <ElevationScroll {...props}>
                <AppBar>
                    <Container maxWidth="xl" className="header-container">
                        <Toolbar disableGutters className="toolbar">
                            <Typography variant="h6" className="logo" component={Link} href="/">
                                Gold
                            </Typography>
                            <Box className="nav-buttons">{renderHeaderList()}</Box>
                            <IconButton color="inherit" aria-label="cart">
                                <ListIcon fontSize="large" />
                            </IconButton>
                        </Toolbar>
                    </Container>
                </AppBar>
            </ElevationScroll>
        </>
    );
};

export default Header;
