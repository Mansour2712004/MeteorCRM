export interface Deal {
  id: string;
  title: string;
  value: number;
  stage: string;
  expectedCloseDate?: string;
  notes?: string;
  customerId: string;
  customerName: string;
  createdAt: string;
}

export interface CreateDealRequest {
  title: string;
  value: number;
  expectedCloseDate?: string;
  notes?: string;
  customerId: string;
}

export interface UpdateDealRequest {
  title: string;
  value: number;
  stage: number;
  expectedCloseDate?: string;
  notes?: string;
}