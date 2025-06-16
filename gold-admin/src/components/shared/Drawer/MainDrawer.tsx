import React, { JSX } from 'react';
import {
    Toolbar,
    Box,
    List,
    ListItem,
    ListItemButton,
    ListItemIcon,
    ListItemText,
    Typography,
} from '@mui/material';
import { NavLink, useMatch, useResolvedPath } from 'react-router-dom';
import clsx from 'clsx';

import menuItems from './menu-items';

type CustomNavLinkType = {
    to: string;
    icon: JSX.Element;
    title: string;
};

const MainDrawer: React.FC = () => {
    return (
        <Box className="main-drawer">
            <Toolbar>
                <Typography variant="h6" component="h6">
                    Yen Sao Gold Admin
                </Typography>
            </Toolbar>
            <List className="main-drawer__list">
                {menuItems.items.map((group) => (
                    <Box key={group.id}>
                        <Typography
                            className="main-drawer__list-header"
                            variant="subtitle2"
                            // color="textSecondary"
                        >
                            {group.title}
                        </Typography>
                        {group.children?.map((child) => (
                            <ListItem
                                key={child.id}
                                className="main-drawer__list-child"
                                disablePadding
                            >
                                <CustomNavLink
                                    to={child.url}
                                    icon={child.icon}
                                    title={child.title}
                                />
                            </ListItem>
                        ))}
                    </Box>
                ))}
            </List>
        </Box>
    );
};

const CustomNavLink = ({ to, icon, title }: CustomNavLinkType) => {
    const resolved = useResolvedPath(to);
    const match = useMatch({ path: resolved.pathname, end: true });

    return (
        <ListItemButton
            component={NavLink}
            to={to}
            className={clsx('main-drawer__list-item', {
                active: match,
            })}
        >
            <ListItemIcon className="main-drawer__list-icon">{icon}</ListItemIcon>
            <ListItemText
                primary={
                    <Typography variant="subtitle2" className="main-drawer__list-text">
                        {title}
                    </Typography>
                }
            />
        </ListItemButton>
    );
};

export default MainDrawer;
