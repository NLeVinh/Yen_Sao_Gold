import { MESSAGES } from '@/constants/messagesConstant';
import { UI } from '@/constants/uiConstant';

export type MessageKeys = keyof typeof MESSAGES;
export type UIKeys = keyof typeof UI;

export type AuthMessages = typeof MESSAGES.AUTH;
export type AuthUI = typeof UI.AUTH;
export type ProfileMessages = typeof MESSAGES.PROFILE;
export type ProfileUI = typeof UI.PROFILE;
export type ProductUI = typeof UI.PRODUCT;
export type SystemMessages = typeof MESSAGES.SYSTEM;
export type ValidationMessages = typeof MESSAGES.VALIDATION;
export type CommonUI = typeof UI.COMMON;
